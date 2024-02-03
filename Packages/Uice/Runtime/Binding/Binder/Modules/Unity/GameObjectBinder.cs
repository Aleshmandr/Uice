using UnityEngine;

namespace Uice
{
	public class GameObjectBinder : ComponentBinder
	{
		[SerializeField] private BindingInfo objectName = BindingInfo.Variable<object>();
		[SerializeField] private BindingInfo isActive = BindingInfo.Variable<bool>();

		protected override void Awake()
		{
			base.Awake();
			
			RegisterVariable<object>(objectName).OnChanged(HandleNameChange);
			RegisterVariable<bool>(isActive).OnChanged(HandleActiveStateChange);
		}

		private void HandleNameChange(object newValue)
		{
			gameObject.name = newValue != null ? newValue.ToString() : string.Empty;
		}

		private void HandleActiveStateChange(bool newValue)
		{
			gameObject.SetActive(newValue);
		}
	}
}
