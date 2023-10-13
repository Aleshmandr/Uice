using System;
using System.Threading.Tasks;
using Uice.Utils;

namespace Uice
{
	public class PanelShowLauncher : IPanelShowLauncher
	{
		private readonly Type panelType;
		private readonly Func<PanelShowSettings, Task> showCallback;

		private IContext context;
		private PanelPriority? priority;
		private ITransition showTransition;
		private ITransition hideTransition;

		public PanelShowLauncher(Type panelType, Func<PanelShowSettings, Task> showCallback)
		{
			this.showCallback = showCallback;
			this.panelType = panelType;
		}

		public IPanelShowLauncher WithContext(IContext context)
		{
			this.context = context;
			return this;
		}

		public IPanelShowLauncher WithPriority(PanelPriority priority)
		{
			this.priority = priority;
			return this;
		}

		public IPanelShowLauncher WithShowTransition(ITransition transition)
		{
			showTransition = transition;
			return this;
		}

		public IPanelShowLauncher WithHideTransition(ITransition transition)
		{
			hideTransition = transition;
			return this;
		}

		public void Execute()
		{
			ExecuteAsync().RunAndForget();
		}

		public async Task ExecuteAsync()
		{
			await showCallback(BuildSettings());
		}

		private PanelShowSettings BuildSettings()
		{
			return new PanelShowSettings(
				panelType,
				context,
				priority,
				showTransition,
				hideTransition);
		}
	}
}
