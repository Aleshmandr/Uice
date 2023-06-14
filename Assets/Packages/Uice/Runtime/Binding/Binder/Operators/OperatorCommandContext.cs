namespace Uice
{
	public class OperatorCommandContext<T> : OperatorContext
	{
		public IObservableCommand<T> Value { get; }

		public OperatorCommandContext(IObservableCommand<T> value)
		{
			Value = value;
		}
	}
}