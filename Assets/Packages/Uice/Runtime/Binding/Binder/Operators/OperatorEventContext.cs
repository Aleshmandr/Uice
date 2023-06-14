namespace Uice
{
	public class OperatorEventContext<T> : OperatorContext
	{
		public IObservableEvent<T> Value { get; }

		public OperatorEventContext(IObservableEvent<T> value)
		{
			Value = value;
		}
	}
}