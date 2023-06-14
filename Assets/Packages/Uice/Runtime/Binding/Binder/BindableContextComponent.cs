using System;
using System.Reflection;
using Uice.Utils;
using UnityEngine;

namespace Uice
{
	public class BindableContextComponent : ContextComponent, IContextInjector
	{
		private static readonly object[] DataArray = new object[1];

		public Type InjectionType => expectedContextType.Type;
		public override Type ExpectedType => expectedContextType.Type;
		public ContextComponent Target => this;

		[TypeConstraint(typeof(IBindableContext<>), true)]
		[SerializeField] protected SerializableType expectedContextType = new();
		[SerializeField] private BindingInfo bindingInfo;

		private VariableBinding<object> binding;
		private MethodInfo setMethod;

		protected override void OnValidate()
		{
			base.OnValidate();

			if (Application.isPlaying == false
				&& expectedContextType != null
				&& expectedContextType.Type != null)
			{
				Type genericType = FindBindableInterface(ExpectedType);
				Type dataType = genericType.GenericTypeArguments[0];
				Type bindingType = typeof(IReadOnlyObservableVariable<>).MakeGenericType(dataType);

				if (bindingInfo.Type != bindingType)
				{
					bindingInfo = new BindingInfo(bindingType);
					BindingInfoTrackerProxy.RefreshBindingInfo();
				}
			}
		}

		protected override void Awake()
		{
			base.Awake();

			binding = new VariableBinding<object>(bindingInfo, this);
			binding.Property.Changed += SetData;
		}

		protected virtual void OnEnable()
		{
			binding.Bind();
		}

		protected virtual void OnDisable()
		{
			binding.Unbind();
		}

		public void SetData(object data)
		{
			if (Context == null)
			{
				if (ExpectedType != null)
				{
					object context = Activator.CreateInstance(ExpectedType);
					setMethod = ExpectedType.GetMethod(nameof(IBindableContext<object>.Set));
					SetData((IContext)context, data);
					Context = (IContext)context;
				}
				else
				{
					Debug.LogError("Expected Type must be set", this);
				}
			}
			else
			{
				SetData(Context, data);
			}
		}

		private static Type FindBindableInterface(Type type)
		{
			Type result = null;

			if (type != null)
			{
				TypeFilter bindableFilter = BindableFilter;
				object filterCriteria = typeof(IBindableContext<>);
				Type[] interfaces = type.FindInterfaces(bindableFilter, filterCriteria);

				if (interfaces.Length > 0)
				{
					result = interfaces[0];
				}
				else
				{
					result = FindBindableInterface(type.BaseType);
				}
			}

			return result;
		}

		private static bool BindableFilter(Type typeObject, object criteria)
		{
			return typeObject.IsGenericType && typeObject.GetGenericTypeDefinition() == (Type)criteria;
		}
		
		private void SetData(IContext context, object data)
		{
			if (setMethod != null)
			{
				DataArray[0] = data;
				setMethod.Invoke(context, DataArray);
			}
		}
	}
}
