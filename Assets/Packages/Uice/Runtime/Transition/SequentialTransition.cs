using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Uice
{
	public class SequentialTransition : ComponentTransition
	{
		[SerializeField] private List<ComponentTransition> transitions;

		protected override void PrepareInternal(Transform target)
		{
			foreach (ComponentTransition current in transitions)
			{
				current.Prepare(target);
			}
		}

		protected override async Task AnimateInternal(Transform target)
		{
			foreach (ComponentTransition current in transitions)
			{
				await current.Animate(target);
			}
		}

		protected override void CleanupInternal(Transform target)
		{
			foreach (ComponentTransition current in transitions)
			{
				current.Cleanup(target);
			}
		}
	}
}