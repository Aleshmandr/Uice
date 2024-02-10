using System.Threading.Tasks;
using Mace;
using UnityEngine;

namespace Uice
{
	public abstract class ComponentTransition : ComponentBinder, ITransition
	{
		public Transform OverrideTarget => overrideTarget;
		
		[SerializeField] private Transform overrideTarget;

		public void Prepare(Transform target)
		{
			PrepareInternal(GetFinalTarget(target));
		}

		public Task Animate(Transform target)
		{
			return AnimateInternal(GetFinalTarget(target));
		}

		public void Cleanup(Transform target)
		{
			CleanupInternal(GetFinalTarget(target));
		}

		protected abstract void PrepareInternal(Transform target);

		protected abstract Task AnimateInternal(Transform target);

		protected abstract void CleanupInternal(Transform target);

		private Transform GetFinalTarget(Transform target)
		{
			return overrideTarget ? overrideTarget : target;
		}
	}
}
