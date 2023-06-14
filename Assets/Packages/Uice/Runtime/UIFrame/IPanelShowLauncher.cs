using System.Threading.Tasks;

namespace Uice
{
	public interface IPanelShowLauncher
	{
		IPanelShowLauncher WithContext(IContext context);
		IPanelShowLauncher WithPriority(PanelPriority priority);
		IPanelShowLauncher WithShowTransition(ITransition transition);
		IPanelShowLauncher WithHideTransition(ITransition transition);
		void Execute();
		Task ExecuteAsync();
	}
}
