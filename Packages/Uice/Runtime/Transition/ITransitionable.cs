using System.Threading.Tasks;

namespace Uice
{
	public delegate void TransitionableEventHandler(ITransitionable transitionable);

	public interface ITransitionable
	{
		event TransitionableEventHandler Showing;
		event TransitionableEventHandler Shown;
		event TransitionableEventHandler Hiding;
		event TransitionableEventHandler Hidden;

		bool IsVisible { get; }

		ITransition GetShowTransition(TransitionData transitionData);
		ITransition GetHideTransition(TransitionData transitionData);
		Task Show(ITransition overrideTransition = null);
		Task Hide(ITransition overrideTransition = null);
	}
}
