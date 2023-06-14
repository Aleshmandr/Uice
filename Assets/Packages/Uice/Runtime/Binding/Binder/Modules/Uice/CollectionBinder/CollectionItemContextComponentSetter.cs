using UnityEngine;

namespace Uice
{
	public class CollectionItemContextComponentSetter : ItemSetter
	{
		public override void SetData(int index, GameObject item, object value)
		{
			var contextComponent = item.GetComponent<CollectionItemContextComponent>();

			if (contextComponent)
			{
				contextComponent.SetData(value);
			}
			else
			{
				Debug.LogError($"Item \"{item.name}\" must contain a {nameof(CollectionItemContextComponent)}.", this);
			}
		}
	}
}
