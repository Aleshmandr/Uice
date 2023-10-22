using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Uice
{
	public class ToVariableBindingAsyncProcessor<TFrom, TTo> : VariableBindingAsyncProcessor<TFrom, TTo>
	{
		private readonly Func<TFrom, Task<TTo>> processFunction;

		public ToVariableBindingAsyncProcessor(BindingInfo bindingInfo, Component viewModel, Func<TFrom, Task<TTo>> processFunction)
			: base(bindingInfo, viewModel)
		{
			this.processFunction = processFunction;
		}

		protected override async Task<TTo> ProcessValueAsync(TFrom value)
		{
			return await processFunction(value);
		}
	}
}