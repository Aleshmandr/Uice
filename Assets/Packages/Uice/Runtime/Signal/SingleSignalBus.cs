using Uice.Utils;

namespace Uice
{
	public class SingleSignalBus : Singleton<SingleSignalBus>
	{
		public SignalBus DefaultSignalBus { get; } = new SignalBus();
	}
}
