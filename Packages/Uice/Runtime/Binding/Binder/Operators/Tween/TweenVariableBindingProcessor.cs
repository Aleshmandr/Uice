using Uice.Tweening;
using UnityEngine;

namespace Uice
{
	public class TweenVariableBindingProcessor<T> : VariableBindingProcessor<T, T>
	{
		public delegate Tweener BuildTweener(Tweener<T>.Getter getter, Tweener<T>.Setter setter, T finalValue);

		private readonly BuildTweener tweenBuilder;

		private Tweener lastTweener;

		public TweenVariableBindingProcessor(BindingInfo bindingInfo, Component viewModel, BuildTweener tweenBuilder)
			: base(bindingInfo, viewModel)
		{
			this.tweenBuilder = tweenBuilder;
		}

		protected override void OnBoundVariableChanged(T newValue)
		{
			lastTweener?.Kill();
			lastTweener = tweenBuilder(() => processedVariable.Value, x => processedVariable.Value = x, newValue);
		}

		protected override T ProcessValue(T value)
		{
			return value;
		}
	}
}
