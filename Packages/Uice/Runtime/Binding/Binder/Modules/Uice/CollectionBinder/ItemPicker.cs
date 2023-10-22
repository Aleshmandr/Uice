using UnityEngine;

namespace Uice
{
	public abstract class ItemPicker : MonoBehaviour
	{
		public abstract ViewModelComponent SpawnItem(int index, IViewModel value, Transform parent);
		public abstract ViewModelComponent ReplaceItem(int index, IViewModel oldValue, IViewModel newValue, ViewModelComponent currentItem, Transform parent);
		public abstract void DisposeItem(int index, ViewModelComponent item);
	}
}
