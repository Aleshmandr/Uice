using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Uice
{
	public class CollectionBinder : ComponentBinder
	{
		public IReadOnlyList<ViewModelComponent> Items => currentItems;

		[SerializeField] private BindingInfo collection = BindingInfo.Collection<object>();
		[SerializeField] private Transform itemsContainer;
		[Header("Dependencies")]
		[SerializeField] protected ItemPicker itemPicker;

		private Transform Container => itemsContainer ? itemsContainer : transform;

		private readonly List<ViewModelComponent> currentItems = new();

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

			RegisterCollection<IViewModel>(collection)
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

		protected virtual void OnCollectionItemAdded(int index, IViewModel value)
		{
			InsertItem(index, value);
		}

		protected virtual void OnCollectionItemMoved(int oldIndex, int newIndex, IViewModel value)
		{
			MoveItem(oldIndex, newIndex);
		}

		protected virtual void OnCollectionItemRemoved(int index, IViewModel value)
		{
			RemoveItem(index);
		}

		protected virtual void OnCollectionItemReplaced(int index, IViewModel oldValue, IViewModel newValue)
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
			ViewModelComponent item = currentItems[index];
			currentItems.RemoveAt(index);
			itemPicker.DisposeItem(index, item);
		}

		private void InsertItem(int index, IViewModel value)
		{
			ViewModelComponent newItem = itemPicker.SpawnItem(index, value, Container);
			currentItems.Insert(index, newItem);
			newItem.transform.SetSiblingIndex(index);
			newItem.ViewModel = value;
		}

		private void MoveItem(int oldIndex, int newIndex)
		{
			ViewModelComponent item = currentItems[oldIndex];
			currentItems.RemoveAt(oldIndex);
			currentItems.Insert(newIndex, item);
			item.transform.SetSiblingIndex(newIndex);
		}
	}
}
