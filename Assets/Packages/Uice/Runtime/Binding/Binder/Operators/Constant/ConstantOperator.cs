using System;
using UnityEngine;

namespace Uice
{
	public abstract class ConstantOperator<T> : Operator
	{
		[SerializeField] private ConstantBindingInfo<T> value = new ConstantBindingInfo<T>(useConstant: true);

		private ObservableVariable<T> exposedVariable;

		protected override void Awake()
		{
			base.Awake();

			exposedVariable = new ObservableVariable<T>();
			Context = new OperatorVariableContext<T>(exposedVariable);

			RegisterVariable<T>(value)
				.OnChanged(OnChanged)
				.OnCleared(OnCleared);
		}

		protected override Type GetInjectionType()
		{
			return typeof(OperatorVariableContext<T>);
		}

		private void OnChanged(T newValue)
		{
			exposedVariable.Value = newValue;
		}

		private void OnCleared()
		{
			exposedVariable.Clear();
		}
	}
}
