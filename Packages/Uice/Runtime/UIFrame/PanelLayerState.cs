using System.Collections.Generic;

namespace Uice
{
	public class PanelLayerState
	{
		public IEnumerable<PanelStateEntry> VisiblePanels { get; }

		public PanelLayerState(IEnumerable<PanelStateEntry> visiblePanels)
		{
			VisiblePanels = visiblePanels;
		}
	}
}
