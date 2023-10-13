namespace Uice
{
	public class CommandContext : Context
	{
		public IObservableCommand Value { get; }

		public CommandContext(IObservableCommand value)
		{
			Value = value;
		}
	}
}