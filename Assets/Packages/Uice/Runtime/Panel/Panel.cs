using UnityEngine;

namespace Uice
{
	public abstract class Panel : Panel<NullContext>
	{

	}

	public abstract class Panel<T> : View<T>, IPanel
		where T : IContext
	{
		public PanelPriority Priority => priority;

		[Header("Panel Properties")]
		[SerializeField] private PanelPriority priority;
	}
}
