using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Uice.Utils;
using UnityEditor;
using UnityEngine;

namespace Uice.Editor
{
	[CustomEditor(typeof(ContextComponent), true)]
	public class ContextComponentEditor : UnityEditor.Editor
	{
		private static readonly Type GenericVariableType = typeof(IReadOnlyObservableVariable<>);
		private static readonly Type GenericCollectionType = typeof(IReadOnlyObservableCollection<>);
		private static readonly Type GenericCommandType = typeof(IObservableCommand<>);
		private static readonly Type CommandType = typeof(IObservableCommand);
		private static readonly Type GenericEventType = typeof(IObservableEvent<>);
		private static readonly Type EventType = typeof(IObservableEvent);

		private ContextComponent ContextComponent => target as ContextComponent;

		private SerializedProperty expectedTypeProperty;
		private SerializedProperty idProperty;
		private bool showContextInfo;

		protected virtual void OnEnable()
		{
			expectedTypeProperty = serializedObject.FindProperty("expectedType");
			idProperty = serializedObject.FindProperty("id");
			BindingInfoTracker.RefreshBindingInfoDrawers();
		}

		protected virtual void OnDisable()
		{
			BindingInfoTracker.RefreshBindingInfoDrawers();
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			DrawBaseInspector();
			DrawChildFields();
			DrawContextInfo();
			serializedObject.ApplyModifiedProperties();
		}

		protected virtual void DrawBaseInspector()
		{
			RefreshType();
			DrawExpectedType();
			DrawId();
		}

		protected virtual void DrawChildFields()
		{
			List<FieldInfo> childFields = new List<FieldInfo>();
			Type type = target.GetType();

			CustomEditor customEditor = GetType().GetCustomAttribute<CustomEditor>();
			FieldInfo fieldInfo = typeof(CustomEditor)
				.GetField("m_InspectedType",
				BindingFlags.NonPublic
				| BindingFlags.Instance
				| BindingFlags.GetField);
			Type baseType = fieldInfo.GetValue(customEditor) as Type;

			while (type != null && type != baseType)
			{
				childFields.InsertRange(0, type.GetFields(
					BindingFlags.DeclaredOnly
					| BindingFlags.Instance
					| BindingFlags.Public
					| BindingFlags.NonPublic));

				type = type.BaseType;
			}

			foreach (FieldInfo field in childFields)
			{
				if (field.IsPublic || field.GetCustomAttribute(typeof(SerializeField)) != null)
				{
					EditorGUILayout.PropertyField(serializedObject.FindProperty(field.Name));
				}
			}
		}

		protected void DrawExpectedType()
		{
			var injector = ContextComponent.GetComponents<IContextInjector>().FirstOrDefault(x => x.Target == ContextComponent);
			bool shouldShowField = injector == null;

			if (shouldShowField)
			{
				EditorGUI.BeginChangeCheck();

				EditorGUILayout.PropertyField(expectedTypeProperty);

				if (EditorGUI.EndChangeCheck())
				{
					BindingInfoTracker.RefreshBindingInfoDrawers();
				}
			}
		}

		protected void DrawId()
		{
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(idProperty);

			if (EditorGUI.EndChangeCheck())
			{
				BindingInfoTracker.RefreshBindingInfoDrawers();
			}
		}

		protected void DrawContextInfo()
		{
			if (ContextComponent.ExpectedType != null)
			{
				var style = GUI.skin.GetStyle("helpbox");
				GUILayout.BeginVertical(style);

				EditorGUI.indentLevel++;

				showContextInfo = EditorGUI.Foldout(
					EditorGUILayout.GetControlRect(),
					showContextInfo,
					ContextComponent.ExpectedType.GetPrettifiedName(),
					true);

				if (showContextInfo)
				{
					GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));

					foreach (BindingEntry entry in BindingUtils.GetAllBindings(ContextComponent.ExpectedType, ContextComponent))
					{
						string propertyType = GetPropertyTypeString(entry);
						EditorGUILayout.LabelField(entry.PropertyName, propertyType);
					}
				}

				DrawContextEdit();
				
				EditorGUI.indentLevel--;

				GUILayout.EndVertical();
			}
		}
		
		private void DrawContextEdit()
		{
			if (GUILayout.Button("Edit")) {
				var scriptName = ContextComponent.ExpectedType.Name;
				string[] guids = AssetDatabase.FindAssets($"t:script {scriptName}");
				if (guids.Length > 0)
				{
					foreach (var guid in guids)
					{
						string assetPath = AssetDatabase.GUIDToAssetPath(guid);
						MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);
						if (script != null && script.GetClass() == ContextComponent.ExpectedType)
						{
							AssetDatabase.OpenAsset(script);
							return;
						}
					}
				} 
				Debug.LogError($"Can't find {scriptName}.cs to edit. Does the class name match the file name?");
			}
		}

		private static string GetPropertyTypeString(BindingEntry entry)
		{
			string propertyType = string.Empty;

			if (entry.ObservableType.ImplementsOrDerives(GenericVariableType))
			{
				propertyType = $": Variable<{entry.GenericArgument.GetPrettifiedName()}>";
			}
			else if (entry.ObservableType.ImplementsOrDerives(GenericCollectionType))
			{
				propertyType = $": Collection<{entry.GenericArgument.GetPrettifiedName()}>";
			}
			else if (entry.ObservableType.ImplementsOrDerives(GenericCommandType))
			{
				propertyType = $": Command<{entry.GenericArgument.GetPrettifiedName()}>";
			}
			else if (entry.ObservableType.ImplementsOrDerives(GenericEventType))
			{
				propertyType = $": Event<{entry.GenericArgument.GetPrettifiedName()}>";
			}
			else if (entry.ObservableType.ImplementsOrDerives(CommandType))
			{
				propertyType = ": Command";
			}
			else if (entry.ObservableType.ImplementsOrDerives(EventType))
			{
				propertyType = ": Event";
			}

			return propertyType;
		}

		private void RefreshType()
		{
			var injectors = ContextComponent.GetComponents<IContextInjector>();

			foreach (IContextInjector current in injectors)
			{
				if (current.Target == ContextComponent && current.InjectionType != ContextComponent.ExpectedType)
				{
					expectedTypeProperty.GetValue<SerializableType>().Type = current.InjectionType;
				}
			}
		}
	}
}
