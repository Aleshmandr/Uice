namespace Uice
{
	public class OperatorCollectionContext<T> : OperatorContext
	{
		public IReadOnlyObservableCollection<T> Value { get; }

		public OperatorCollectionContext(IReadOnlyObservableCollection<T> value)
		{
			Value = value;
		}
	}
}
