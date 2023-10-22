using System.Collections.Generic;
using UnityEngine;

namespace Uice
{
	public class BindingTracker
	{
		private readonly Component viewModel;
		private readonly List<Binding> bindings;

		public BindingTracker(Component viewModel)
		{
			this.viewModel = viewModel;
			bindings = new List<Binding>();
		}

		public void Bind()
		{
			foreach (Binding current in bindings)
			{
				current.Bind();
			}
		}

		public void Unbind()
		{
			foreach (Binding current in bindings)
			{
				current.Unbind();
			}
		}

		public VariableBindingSubscriber<T> RegisterVariable<T>(BindingInfo info)
		{
			var binding = new VariableBinding<T>(info, viewModel);
			bindings.Add(binding);
			return new VariableBindingSubscriber<T>(binding);
		}

		public CollectionBindingSubscriber<T> RegisterCollection<T>(BindingInfo info)
		{
			var binding = new CollectionBinding<T>(info, viewModel);
			bindings.Add(binding);
			return new CollectionBindingSubscriber<T>(binding);
		}

		public EventBindingSubscriber RegisterEvent(BindingInfo info)
		{
			var binding = new EventBinding(info, viewModel);
			bindings.Add(binding);
			return new EventBindingSubscriber(binding);
		}

		public EventBindingSubscriber<T> RegisterEvent<T>(BindingInfo info)
		{
			var binding = new EventBinding<T>(info, viewModel);
			bindings.Add(binding);
			return new EventBindingSubscriber<T>(binding);
		}

		public CommandBindingSubscriber RegisterCommand(BindingInfo info)
		{
			var binding = new CommandBinding(info, viewModel);
			bindings.Add(binding);
			return new CommandBindingSubscriber(binding);
		}

		public CommandBindingSubscriber<T> RegisterCommand<T>(BindingInfo info)
		{
			var binding = new CommandBinding<T>(info, viewModel);
			bindings.Add(binding);
			return new CommandBindingSubscriber<T>(binding);
		}
	}
}
