namespace Uice
{
	public class OperatorVariableContext<T> : OperatorContext
	{
		public IReadOnlyObservableVariable<T> Value { get; }

		public OperatorVariableContext(IReadOnlyObservableVariable<T> value)
		{
			Value = value;
		}
	}
}