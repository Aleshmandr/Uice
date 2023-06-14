using UnityEngine;

namespace Uice
{
	[RequireComponent(typeof(CollectionItemContextComponentPicker))]
	[RequireComponent(typeof(CollectionItemContextComponentSetter))]
	public class PrefabCollectionBinder : CollectionBinder
	{
		protected virtual void Reset()
		{
			RetrieveItemHandlers();
		}

		protected override void OnValidate()
		{
			base.OnValidate();

			RetrieveItemHandlers();
		}

		private void RetrieveItemHandlers()
		{
			itemPicker = GetComponent<CollectionItemContextComponentPicker>();
			itemSetter = GetComponent<CollectionItemContextComponentSetter>();
		}
	}
}
