namespace Uice
{
	public class UIFrameState
	{
		public WindowLayerState WindowLayerState { get; }
		public PanelLayerState PanelLayerState { get; }

		public UIFrameState(WindowLayerState windowLayerState, PanelLayerState panelLayerState)
		{
			WindowLayerState = windowLayerState;
			PanelLayerState = panelLayerState;
		}
	}
}
