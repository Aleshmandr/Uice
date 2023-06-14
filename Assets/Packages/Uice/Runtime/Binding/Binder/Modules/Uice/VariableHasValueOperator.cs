using System;
using UnityEngine;

namespace Uice
{
	public class VariableHasValueOperator : Operator
	{
		[SerializeField] private BindingInfo variable = BindingInfo.Variable<object>();

		private VariableBinding<object> variableBinding;
		private ObservableVariable<bool> exposedVariable;

		protected override void Awake()
		{
			base.Awake();

			exposedVariable = new ObservableVariable<bool>();
			Context = new OperatorVariableContext<bool>(exposedVariable);

			variableBinding = RegisterVariable<object>(variable)
				.OnChanged(OnVariableChanged)
				.OnCleared(OnVariableCleared)
				.GetBinding();
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			exposedVariable.Value = false;
		}

		protected override Type GetInjectionType()
		{
			return typeof(OperatorVariableContext<bool>);
		}

		private void OnVariableChanged(object newValue)
		{
			Refresh();
		}

		private void OnVariableCleared()
		{
			Refresh();
		}

		private void Refresh()
		{
			exposedVariable.Value = variableBinding.IsBound && variableBinding.Property.HasValue;
		}
	}
}
