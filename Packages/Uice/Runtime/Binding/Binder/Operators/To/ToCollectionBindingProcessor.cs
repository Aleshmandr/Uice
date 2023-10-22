using System;
using UnityEngine;

namespace Uice
{
	public class ToCollectionBindingProcessor<TFrom, TTo> : CollectionBindingProcessor<TFrom, TTo>
	{
		private readonly Func<TFrom, TTo> processFunction;

		public ToCollectionBindingProcessor(BindingInfo bindingInfo, Component viewModel, Func<TFrom, TTo> processFunction)
			: base(bindingInfo, viewModel)
		{
			this.processFunction = processFunction;
		}

		protected override TTo ProcessValue(TFrom newValue, TFrom oldValue, bool isNewItem)
		{
			return processFunction(newValue);
		}
	}
}