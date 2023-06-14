namespace Uice
{
	public abstract class OperatorContext : IContext
	{
		public bool IsEnabled { get; private set; }

		public void Enable()
		{
			IsEnabled = true;
		}

		public void Disable()
		{
			IsEnabled = false;
		}
	}
}
