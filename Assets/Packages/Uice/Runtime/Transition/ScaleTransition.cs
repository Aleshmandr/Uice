using System.Threading.Tasks;
using Uice.Tweening;
using UnityEngine;

namespace Uice
{
	public class ScaleTransition : ComponentTransition
	{
		[SerializeField] private Vector3 origin;
		[SerializeField] private Vector3 destiny;
		[SerializeField] private float duration;
		[SerializeField] private Ease ease;

		protected override void PrepareInternal(Transform target)
		{
			Tween.Kill(target);
			target.localScale = origin;
		}

		protected override async Task AnimateInternal(Transform target)
		{
			bool isTweenDone = false;

			target.TweenLocalScale(destiny, duration)
				.SetEase(ease)
				.Completed += t => isTweenDone = true;

			while (isTweenDone == false)
			{
				await Task.Yield();
			}
		}

		protected override void CleanupInternal(Transform target)
		{
			
		}
	}
}