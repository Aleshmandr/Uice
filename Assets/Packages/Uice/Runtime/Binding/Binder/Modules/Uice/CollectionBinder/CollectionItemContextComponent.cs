using System;
using System.Reflection;
using UnityEngine;

namespace Uice
{
	public class CollectionItemContextComponent : ContextComponent, IContextInjector
	{
		private static readonly object[] DataArray = new object[1];

		public Type InjectionType => expectedContextType.Type;
		public override Type ExpectedType => expectedContextType.Type;
		public ContextComponent Target => this;

		[TypeConstraint(typeof(IBindableContext<>), true)]
		[SerializeField] protected SerializableType expectedContextType;
		
		private MethodInfo setMethod;

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
