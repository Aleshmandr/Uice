using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Uice.Utils;
using UnityEditor;
using UnityEngine;

namespace Uice.Editor
{
    [CustomEditor(typeof(ViewModelComponent), true)]
    public class ViewModelComponentEditor : UnityEditor.Editor
    {
        private static readonly Type GenericVariableType = typeof(IReadOnlyObservableVariable<>);
        private static readonly Type GenericCollectionType = typeof(IReadOnlyObservableCollection<>);
        private static readonly Type GenericCommandType = typeof(IObservableCommand<>);
        private static readonly Type CommandType = typeof(IObservableCommand);
        private static readonly Type GenericEventType = typeof(IObservableEvent<>);
        private static readonly Type EventType = typeof(IObservableEvent);

        private ViewModelComponent ViewModelComponent => target as ViewModelComponent;

        private SerializedProperty expectedTypeProperty;
        private SerializedProperty idProperty;
        private bool showViewModelInfo;

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
            DrawViewModelInfo();
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
            var injector = ViewModelComponent.GetComponents<IViewModelInjector>().FirstOrDefault(x => x.Target == ViewModelComponent);
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

        protected void DrawViewModelInfo()
        {
            if (ViewModelComponent.ExpectedType != null)
            {
                var style = GUI.skin.GetStyle("helpbox");
                GUILayout.BeginVertical(style);

                EditorGUI.indentLevel++;

                showViewModelInfo = EditorGUI.Foldout(
                    EditorGUILayout.GetControlRect(),
                    showViewModelInfo,
                    ViewModelComponent.ExpectedType.GetPrettifiedName(),
                    true);

                if (showViewModelInfo)
                {
                    GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));

                    foreach (BindingEntry entry in BindingUtils.GetAllBindings(ViewModelComponent.ExpectedType, ViewModelComponent))
                    {
                        string propertyType = GetPropertyTypeString(entry);
                        EditorGUILayout.LabelField(entry.PropertyName, propertyType);
                    }
                }

                DrawViewModelEdit();

                EditorGUI.indentLevel--;

                GUILayout.EndVertical();
            }
        }

        private void DrawViewModelEdit()
        {
            if (GUILayout.Button("Edit"))
            {

                var scriptName = ViewModelComponent.ExpectedType.IsGenericType ?
                    ViewModelComponent.ExpectedType.Name.Split('`')[0] : 
                    ViewModelComponent.ExpectedType.Name;
                
                string[] guids = AssetDatabase.FindAssets($"t:script {scriptName}");
                if (guids.Length > 0)
                {
                    foreach (var guid in guids)
                    {
                        string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                        MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);
                        if (script != null &&  ViewModelComponent.ExpectedType.IsGenericType || script.GetClass() == ViewModelComponent.ExpectedType)
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
            } else if (entry.ObservableType.ImplementsOrDerives(GenericCollectionType))
            {
                propertyType = $": Collection<{entry.GenericArgument.GetPrettifiedName()}>";
            } else if (entry.ObservableType.ImplementsOrDerives(GenericCommandType))
            {
                propertyType = $": Command<{entry.GenericArgument.GetPrettifiedName()}>";
            } else if (entry.ObservableType.ImplementsOrDerives(GenericEventType))
            {
                propertyType = $": Event<{entry.GenericArgument.GetPrettifiedName()}>";
            } else if (entry.ObservableType.ImplementsOrDerives(CommandType))
            {
                propertyType = ": Command";
            } else if (entry.ObservableType.ImplementsOrDerives(EventType))
            {
                propertyType = ": Event";
            }

            return propertyType;
        }

        private void RefreshType()
        {
            var injectors = ViewModelComponent.GetComponents<IViewModelInjector>();

            foreach (IViewModelInjector current in injectors)
            {
                if (current.Target == ViewModelComponent && current.InjectionType != ViewModelComponent.ExpectedType)
                {
                    expectedTypeProperty.GetValue<SerializableType>().Type = current.InjectionType;
                }
            }
        }
    }
}
