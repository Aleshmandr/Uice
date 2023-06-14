namespace Uice
{
	public interface IContext
	{
		bool IsEnabled { get; }
		void Enable();
		void Disable();
	}
}
