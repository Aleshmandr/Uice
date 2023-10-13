using System.Collections.Generic;

namespace Uice
{
	public class WindowLayerState
	{
		public IEnumerable<WindowStateEntry> WindowQueue { get; }
		public IEnumerable<WindowStateEntry> WindowHistory { get; }

		public WindowLayerState(IEnumerable<WindowStateEntry> windowQueue, IEnumerable<WindowStateEntry> windowHistory)
		{
			WindowQueue = windowQueue;
			WindowHistory = windowHistory;
		}
	}
}
