using Uice.Tweening;
using UnityEngine;

namespace Uice
{
	public class Vector3TweenOperator : TweenOperator<Vector3>
	{
		protected override Tweener<Vector3> BuildTweener(Tweener<Vector3>.Getter getter, Tweener<Vector3>.Setter setter, Vector3 finalValue, float duration)
		{
			return Tween.To(getter, setter, finalValue, duration);
		}
	}
}