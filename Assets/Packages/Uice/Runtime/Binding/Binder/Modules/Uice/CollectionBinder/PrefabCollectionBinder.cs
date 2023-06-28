using UnityEngine;

namespace Uice
{
	[RequireComponent(typeof(CollectionItemContextComponentPicker))]
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
		}
	}
}
