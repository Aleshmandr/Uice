using System.Threading.Tasks;
using Uice.Tweening;
using UnityEngine;

namespace Uice
{
    public class MoveRectTransition : ComponentTransition
    {
        [SerializeField] private Vector2 anchoredOrigin;
        [SerializeField] private Vector2 anchoredDestiny;
        [SerializeField] private float duration = 0.3f;
        [SerializeField] private Ease ease = Ease.InOutSine;

        protected override void PrepareInternal(Transform target)
        {
            Tween.Kill(target);
            if (target is RectTransform targetRectTransform)
            {
                targetRectTransform.anchoredPosition = anchoredOrigin;
            }
        }

        protected override async Task AnimateInternal(Transform target)
        {
            if (target is RectTransform targetRectTransform)
            {
                bool isTweenDone = false;

                targetRectTransform.TweenAnchoredPosition(anchoredDestiny, duration)
                    .SetEase(ease)
                    .Completed += t => isTweenDone = true;

                while (isTweenDone == false)
                {
                    await Task.Yield();
                }
            }
        }

        protected override void CleanupInternal(Transform target) { }
    }
}
