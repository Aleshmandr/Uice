using System;
using Mace;
using Uice.Utils;

namespace Uice
{
    public struct BindingEntry
    {
        public ViewModelComponent ViewModelComponent { get; }
        public string PropertyName { get; }
        public bool NeedsToBeBoxed { get; }
        public Type ObservableType { get; }
        public Type GenericArgument { get; }

        public BindingEntry(
            ViewModelComponent viewModelComponent,
            string propertyName,
            bool needsToBeBoxed,
            Type observableType,
            Type genericArgument)
        {
            ViewModelComponent = viewModelComponent;
            PropertyName = propertyName;
            NeedsToBeBoxed = needsToBeBoxed;
            ObservableType = observableType;
            GenericArgument = genericArgument;
        }
    }
}
