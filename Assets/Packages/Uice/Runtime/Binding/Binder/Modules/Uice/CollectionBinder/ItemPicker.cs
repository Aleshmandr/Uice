using UnityEngine;

namespace Uice
{
	public abstract class ItemPicker : MonoBehaviour
	{
		public abstract ContextComponent SpawnItem(int index, IContext value, Transform parent);
		public abstract ContextComponent ReplaceItem(int index, IContext oldValue, IContext newValue, ContextComponent currentItem, Transform parent);
		public abstract void DisposeItem(int index, ContextComponent item);
	}
}
