using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Uice
{
	public class WindowTransitionPicker : TransitionPicker
	{
		[Serializable]
		public class TransitionEntry
		{
			[TypeConstraint(typeof(IWindow), true)] public SerializableType window;
			public ComponentTransition transition;
		}

		public override ComponentTransition DefaultTransition
		{
			get => defaultTransition;
			set => defaultTransition = value;
		}

		private ComponentTransition CurrentTransition => selectedTransition == null ? defaultTransition : selectedTransition;

		[SerializeField] private ComponentTransition defaultTransition = default;
		[SerializeField] private List<TransitionEntry> transitions = default;
		private ComponentTransition selectedTransition;

		public override ITransition SelectTransition(TransitionData transitionData)
		{
			if (transitionData is WindowTransitionData windowTransitionData)
			{
				selectedTransition = transitions.FirstOrDefault(x => x.window.Type == windowTransitionData.TargetWindow.GetType())?.transition;
			}

			return this;
		}

		protected override void PrepareInternal(Transform target)
		{
			if (CurrentTransition != null)
			{
				CurrentTransition.Prepare(target);
			}
		}

		protected override async Task AnimateInternal(Transform target)
		{
			if (CurrentTransition != null)
			{
				await CurrentTransition.Animate(target);
			}
		}

		protected override void CleanupInternal(Transform target)
		{
			if (CurrentTransition != null)
			{
				CurrentTransition.Cleanup(target);
			}
		}
	}
}