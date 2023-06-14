using System;

namespace Uice
{
	public class PanelHideSettings : IViewHideSettings
	{
		public Type ViewType { get; }
		public ITransition HideTransition { get; }
		public ITransition ShowTransition { get;  }

		public PanelHideSettings(Type viewType, ITransition hideTransition, ITransition showTransition)
		{
			ViewType = viewType;
			HideTransition = hideTransition;
			ShowTransition = showTransition;
		}
	}
}
