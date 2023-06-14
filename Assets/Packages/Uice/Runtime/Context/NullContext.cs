namespace Uice
{
	public sealed class NullContext : IContext
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