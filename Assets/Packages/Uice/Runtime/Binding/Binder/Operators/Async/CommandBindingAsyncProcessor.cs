using System.Threading.Tasks;
using UnityEngine;

namespace Uice
{
	public abstract class CommandBindingAsyncProcessor<TFrom, TTo> : IBindingProcessor
	{
		public IContext Context { get; }

		protected readonly CommandBinding<TTo> commandBinding;

		protected CommandBindingAsyncProcessor(BindingInfo bindingInfo, Component context)
		{
			commandBinding = new CommandBinding<TTo>(bindingInfo, context);
			ObservableCommand<TFrom> convertedCommand = new ObservableCommand<TFrom>(commandBinding.Property.CanExecute);
			Context = new OperatorCommandContext<TFrom>(convertedCommand);
			convertedCommand.ExecuteRequested += ProcessedCommandExecuteRequestedHandler;
		}

		public virtual void Bind()
		{
			commandBinding.Bind();
		}

		public virtual void Unbind()
		{
			commandBinding.Unbind();
		}

		protected abstract Task<TTo> ProcessValueAsync(TFrom value);

		protected virtual async void ProcessedCommandExecuteRequestedHandler(TFrom parameter)
		{
			TTo result = await ProcessValueAsync(parameter);

			commandBinding.Property.Execute(result);
		}
	}
}