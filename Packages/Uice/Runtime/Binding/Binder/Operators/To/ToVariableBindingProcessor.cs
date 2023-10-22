using System;
using UnityEngine;

namespace Uice
{
	public class ToVariableBindingProcessor<TFrom, TTo> : VariableBindingProcessor<TFrom, TTo>
	{
		private readonly Func<TFrom, TTo> processFunction;

		public ToVariableBindingProcessor(BindingInfo bindingInfo, Component viewModel, Func<TFrom, TTo> processFunction)
			: base(bindingInfo, viewModel)
		{
			this.processFunction = processFunction;
		}

		public void Refresh()
		{
			if (variableBinding.IsBound && variableBinding.Property.HasValue)
			{
				OnBoundVariableChanged(variableBinding.Property.Value);
			}
		}

		protected override TTo ProcessValue(TFrom value)
		{
			return processFunction(value);
		}
	}
}
