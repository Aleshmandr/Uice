namespace Uice
{
	public interface IObservableVariable<T> : IReadOnlyObservableVariable<T>
	{
		new T Value { get; set; }
	}
}

