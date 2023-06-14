using System;
using Uice.Utils;

namespace Uice
{
	public struct BindingEntry
	{
		public ContextComponent ContextComponent { get; }
		public string PropertyName { get; }
		public BindingPath Path => path ?? (path = new BindingPath(ContextComponent.Id, PropertyName)).Value;
		public bool NeedsToBeBoxed { get; }
		public Type ObservableType { get; }
		public Type GenericArgument { get; }

		private BindingPath? path;

		public BindingEntry(
			ContextComponent contextComponent,
			string propertyName,
			bool needsToBeBoxed,
			Type observableType,
			Type genericArgument)
		{
			ContextComponent = contextComponent;
			PropertyName = propertyName;
			NeedsToBeBoxed = needsToBeBoxed;
			ObservableType = observableType;
			GenericArgument = genericArgument;
			path = null;
		}
	}
}
