using System.Collections.Generic;
using System.Threading.Tasks;
using Uice.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Uice
{
	public class WindowParaLayer : MonoBehaviour
	{
		internal delegate void WindowParaLayerEventHandler();

		internal event WindowParaLayerEventHandler BackgroundClicked;

		[SerializeField] private Widget backgroundWidget;

		private readonly List<GameObject> containedViews = new List<GameObject>();
		private Button backgroundButton;
		private bool isHiding;

		private void Awake()
		{
			InitializeShadowButton();
		}

		internal void SetBackgroundWidget(Widget backgroundWidget)
		{
			this.backgroundWidget = backgroundWidget;
			InitializeShadowButton();
		}

		public void AddView(Transform viewTransform)
		{
			viewTransform.SetParent(transform, false);
			containedViews.Add(viewTransform.gameObject);
		}

		public void RefreshBackground()
		{
			bool shouldShowBackground = ShouldBackgroundBeVisible();
			if (IsBackgroundVisible() != shouldShowBackground)
			{
				if (shouldShowBackground)
				{
					ShowBackground();
				} else
				{
					HideBackground();
				}
			}
		}

		public void ShowBackground()
		{
			if (backgroundWidget)
			{
				backgroundWidget.transform.SetAsLastSibling();
				backgroundWidget.Show().RunAndForget();
			}
		}

		public void HideBackground()
		{
			HideBackgroundAsync().RunAndForget();
		}

		private async Task HideBackgroundAsync()
		{
			if (backgroundWidget)
			{
				isHiding = true;

				await backgroundWidget.Hide();

				isHiding = false;
			}
		}

		private bool IsBackgroundVisible()
		{
			return backgroundWidget.IsVisible && !isHiding;
		}

		private bool ShouldBackgroundBeVisible()
		{
			if (backgroundWidget)
			{
				for (int i = 0; i < containedViews.Count; i++)
				{
					if (containedViews[i] && containedViews[i].activeSelf)
					{
						return !containedViews[i].TryGetComponent(out ITransitionable transitionable) || transitionable.IsVisible;
					}
				}
			}

			return false;
		}

		private void InitializeShadowButton()
		{
			if (backgroundWidget)
			{
				backgroundButton = backgroundWidget.GetOrAddComponent<Button>();
				backgroundButton.transition = Selectable.Transition.None;
				backgroundButton.onClick.RemoveListener(OnBackgroundClicked);
				backgroundButton.onClick.AddListener(OnBackgroundClicked);
			}
		}

		private void OnBackgroundClicked()
		{
			BackgroundClicked?.Invoke();
		}
	}
}
