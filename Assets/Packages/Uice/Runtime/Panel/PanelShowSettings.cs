using System;

namespace Uice
{
	public class PanelShowSettings : IViewShowSettings
	{
		public Type ViewType { get; }
		public IContext Context { get; }
		public PanelPriority? Priority { get; }
		public ITransition ShowTransition { get; }
		public ITransition HideTransition { get; }

		public PanelShowSettings(Type viewType, IContext context, PanelPriority? priority,ITransition showTransition, ITransition hideTransition)
		{
			ViewType = viewType;
			Context = context;
			Priority = priority;
			ShowTransition = showTransition;
			HideTransition = hideTransition;
		}
	}
}
