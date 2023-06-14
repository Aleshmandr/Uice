using UnityEngine;

namespace Uice
{
	public class InteractionBlockingTracker : MonoBehaviour
	{
		public delegate void StateChangeEventHandler(bool isInteractable);

		public event StateChangeEventHandler IsInteractableChanged;

		public bool IsInteractable
		{
			get => isInteractable;

			set
			{
				if (isInteractable != value)
				{
					isInteractable = value;
					OnIsInteractableChanged(isInteractable);
				}
			}
		}

		private bool isInteractable = true;

		protected virtual void OnIsInteractableChanged(bool newState)
		{
			IsInteractableChanged?.Invoke(newState);
		}
	}
}
