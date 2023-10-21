﻿using System;
using UnityEngine;

namespace Uice
{
	public class IntComparerOperator : Operator
	{
		[SerializeField] private BindingInfo operandA = BindingInfo.Variable<int>();
		[SerializeField] private MathComparisonType operation;
		[SerializeField] private ConstantBindingInfo<int> operandB = new ConstantBindingInfo<int>();

		private int A => operandABinding.Property.GetValue(0);
		private int B => operandBBinding.Property.GetValue(0);

		private ObservableVariable<bool> result;
		private VariableBinding<int> operandABinding;
		private VariableBinding<int> operandBBinding;

		protected override void Awake()
		{
			base.Awake();

			result = new ObservableVariable<bool>();
			Context = new OperatorVariableContext<bool>(result);;

			operandABinding = RegisterVariable<int>(operandA).OnChanged(OnOperandChanged).GetBinding();
			operandBBinding = RegisterVariable<int>(operandB).OnChanged(OnOperandChanged).GetBinding();
		}

		protected override Type GetInjectionType()
		{
			return typeof(OperatorVariableContext<bool>);
		}

		private void OnOperandChanged(int newValue)
		{
			if (operandABinding.IsBound && operandBBinding.IsBound)
			{
				result.Value = Evaluate();
			}
		}

		private bool Evaluate()
		{
			bool result = false;

			switch (operation)
			{
				case MathComparisonType.Equals: result = A == B; break;
				case MathComparisonType.NotEquals: result = A != B; break;
				case MathComparisonType.Greater: result = A > B; break;
				case MathComparisonType.GreaterOrEquals: result = A >= B; break;
				case MathComparisonType.Less: result = A < B; break;
				case MathComparisonType.LessOrEquals: result = A <= B; break;
			}

			return result;
		}
	}
}