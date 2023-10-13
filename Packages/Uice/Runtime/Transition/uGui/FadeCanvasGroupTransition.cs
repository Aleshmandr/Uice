using System.Threading.Tasks;
using Uice.Utils;
using Uice.Tweening;
using UnityEngine;

namespace Uice
{
	public class FadeCanvasGroupTransition : ComponentTransition
	{
		public enum FadeType
		{
			In,
			Out
		}

		internal FadeType FadeTypeInternal { get => fadeType; set => fadeType = value; }

		[SerializeField] private FadeType fadeType = FadeType.In;
		[SerializeField] private float duration = 0.3f;
		[SerializeField] private Ease ease = Ease.InOutSine;

		protected override void PrepareInternal(Transform target)
		{
			CanvasGroup canvasGroup = target.GetOrAddComponent<CanvasGroup>();
			Tween.Kill(canvasGroup);

			if (fadeType == FadeType.In)
			{
				canvasGroup.alpha = 0;
			}
			else
			{
				canvasGroup.alpha = 1;
			}
		}

		protected override async Task AnimateInternal(Transform target)
		{
			float valueTarget = fadeType == FadeType.In ? 1 : 0;
			CanvasGroup canvasGroup = target.GetOrAddComponent<CanvasGroup>();
			bool isTweenDone = false;

			canvasGroup.Fade(valueTarget, duration)
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
