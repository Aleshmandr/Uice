using UnityEngine;

namespace Uice
{
	public abstract class EventBindingProcessor<TFrom, TTo> : IBindingProcessor
	{
		public IViewModel ViewModel { get; }

		protected readonly EventBinding<TFrom> eventBinding;
		protected readonly ObservableEvent<TTo> processedEvent;
		
		public EventBindingProcessor(BindingInfo bindingInfo, Component viewModel)
		{
			processedEvent = new ObservableEvent<TTo>();
			ViewModel = new OperatorEventViewModel<TTo>(processedEvent);
			eventBinding = new EventBinding<TFrom>(bindingInfo, viewModel);
			eventBinding.Property.Raised += BoundEventRaisedHandler; 
		}

		public virtual void Bind()
		{
			eventBinding.Bind();
		}

		public virtual void Unbind()
		{
			eventBinding.Unbind();
		}

		protected abstract TTo ProcessValue(TFrom value);
		
		protected virtual void BoundEventRaisedHandler(TFrom eventData)
		{
			processedEvent.Raise(ProcessValue(eventData));
		}
	}
}