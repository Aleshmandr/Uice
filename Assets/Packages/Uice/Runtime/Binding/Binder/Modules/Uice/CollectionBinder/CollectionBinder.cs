using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Uice
{
	public class CollectionBinder : ComponentBinder
	{
		public IReadOnlyList<ContextComponent> Items => currentItems;

		[SerializeField] private BindingInfo collection = BindingInfo.Collection<object>();
		[SerializeField] private Transform itemsContainer = default;
		[Header("Dependencies")]
		[SerializeField] protected ItemPicker itemPicker = default;

		private Transform Container => itemsContainer ? itemsContainer : transform;

		private readonly List<ContextComponent> currentItems = new();

		protected virtual void OnValidate()
		{
			if (itemsContainer == null)
			{
				itemsContainer = transform;
			}
		}

		protected override void Awake()
		{
			base.Awake();

			Assert.IsNotNull(itemPicker, $"A {nameof(CollectionBinder)} needs an {nameof(ItemPicker)} to work.");

			RegisterCollection<IContext>(collection)
				.OnReset(OnCollectionReset)
				.OnItemAdded(OnCollectionItemAdded)
				.OnItemMoved(OnCollectionItemMoved)
				.OnItemRemoved(OnCollectionItemRemoved)
				.OnItemReplaced(OnCollectionItemReplaced);
		}

		protected virtual void OnCollectionReset()
		{
			ClearItems();
		}

		protected virtual void OnCollectionItemAdded(int index, IContext value)
		{
			InsertItem(index, value);
		}

		protected virtual void OnCollectionItemMoved(int oldIndex, int newIndex, IContext value)
		{
			MoveItem(oldIndex, newIndex);
		}

		protected virtual void OnCollectionItemRemoved(int index, IContext value)
		{
			RemoveItem(index);
		}

		protected virtual void OnCollectionItemReplaced(int index, IContext oldValue, IContext newValue)
		{
			currentItems[index] = itemPicker.ReplaceItem(
				index,
				oldValue,
				newValue,
				currentItems[index],
				Container);
		}

		private void ClearItems()
		{
			for (int i = currentItems.Count - 1; i >= 0; i--)
			{
				RemoveItem(i);
			}
		}

		private void RemoveItem(int index)
		{
			ContextComponent item = currentItems[index];
			currentItems.RemoveAt(index);
			itemPicker.DisposeItem(index, item);
		}

		private void InsertItem(int index, IContext value)
		{
			ContextComponent newItem = itemPicker.SpawnItem(index, value, Container);
			currentItems.Insert(index, newItem);
			newItem.transform.SetSiblingIndex(index);
			newItem.Context = value;
		}

		private void MoveItem(int oldIndex, int newIndex)
		{
			ContextComponent item = currentItems[oldIndex];
			currentItems.RemoveAt(oldIndex);
			currentItems.Insert(newIndex, item);
			item.transform.SetSiblingIndex(newIndex);
		}
	}
}
