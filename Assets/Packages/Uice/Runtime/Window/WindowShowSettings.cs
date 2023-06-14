using System;
using System.Collections.Generic;

namespace Uice
{
	public class WindowShowSettings : IViewShowSettings
	{
		public Type ViewType { get; }
		public IContext Context { get; set; }
		public Dictionary<string, object> Payload { get; }
		public ITransition ShowTransition { get; }
		public ITransition HideTransition { get; }
		public WindowPriority? Priority { get; }
		public Type BackDestinationType { get; }
		public Type StubViewType { get; }

		public WindowShowSettings(
			Type windowType,
			IContext context,
			Dictionary<string, object> payload,
			ITransition showTransition,
			ITransition hideTransition,
			WindowPriority? priority,
			Type backDestinationType,
			Type stubViewType)
		{
			ViewType = windowType;
			Context = context;
			Payload = payload;
			ShowTransition = showTransition;
			HideTransition = hideTransition;
			Priority = priority;
			BackDestinationType = backDestinationType;
			StubViewType = stubViewType;
		}
	}
}
