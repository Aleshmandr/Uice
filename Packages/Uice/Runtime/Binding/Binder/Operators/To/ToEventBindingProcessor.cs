using System;
using UnityEngine;

namespace Uice
{
	public class ToEventBindingProcessor<TFrom, TTo> : EventBindingProcessor<TFrom, TTo>
	{
		private readonly Func<TFrom, TTo> processFunction;

		public ToEventBindingProcessor(BindingInfo bindingInfo, Component viewModel, Func<TFrom, TTo> processFunction)
			: base(bindingInfo, viewModel)
		{
			this.processFunction = processFunction;
		}

		protected override TTo ProcessValue(TFrom value)
		{
			return processFunction(value);
		}
	}
}