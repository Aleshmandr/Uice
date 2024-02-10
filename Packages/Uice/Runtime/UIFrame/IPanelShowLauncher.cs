using System.Threading.Tasks;
using Mace;

namespace Uice
{
	public interface IPanelShowLauncher
	{
		IPanelShowLauncher WithViewModel(IViewModel viewModel);
		IPanelShowLauncher WithPriority(PanelPriority priority);
		IPanelShowLauncher WithShowTransition(ITransition transition);
		IPanelShowLauncher WithHideTransition(ITransition transition);
		void Execute();
		Task ExecuteAsync();
	}
}
