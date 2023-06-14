using Uice.Tweening;
using UnityEngine;

namespace Uice
{
	public class Vector2TweenOperator : TweenOperator<Vector2>
	{
		protected override Tweener<Vector2> BuildTweener(Tweener<Vector2>.Getter getter, Tweener<Vector2>.Setter setter, Vector2 finalValue, float duration)
		{
			return Tween.To(getter, setter, finalValue, duration);
		}
	}
}