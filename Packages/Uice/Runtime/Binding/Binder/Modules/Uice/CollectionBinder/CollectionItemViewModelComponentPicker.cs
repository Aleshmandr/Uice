using System.Collections.Generic;
using Uice.Utils;
using Uice.Pooling;
using UnityEngine;
using UnityEngine.Assertions;

namespace Uice
{
	public class CollectionItemViewModelComponentPicker : ItemPicker
	{
		[SerializeField] private List<ViewModelComponent> prefabs;
		[SerializeField] private ObjectPool pool;

		private PrefabPicker<ViewModelComponent> prefabPicker;
		private bool isInitialized;

		protected virtual void Awake()
		{
			EnsureInitialState();
		}

		public override ViewModelComponent SpawnItem(int index, IViewModel value, Transform parent)
		{
			EnsureInitialState();

			ViewModelComponent bestPrefab = prefabPicker.FindBestPrefab(value);

			Assert.IsNotNull(bestPrefab, $"A suitable prefab could not be found for {value} ({value.GetType().GetPrettifiedName()}).");

			return SpawnItem(bestPrefab, parent);
		}

		public override ViewModelComponent ReplaceItem(int index, IViewModel oldValue, IViewModel newValue, ViewModelComponent currentItem, Transform parent)
		{
			EnsureInitialState();

			ViewModelComponent result = currentItem;

			var prefabForOldValue = prefabPicker.FindBestPrefab(oldValue);
			var prefabForNewValue = prefabPicker.FindBestPrefab(newValue);

			if (ReferenceEquals(prefabForOldValue, prefabForNewValue) == false)
			{
				result = SpawnItem(prefabForNewValue, parent);
			}

			return result;
		}

		public override void DisposeItem(int index, ViewModelComponent item)
		{
			EnsureInitialState();

			if (pool)
			{
				pool.Recycle(item);
			}
			else
			{
				Destroy(item.gameObject);
			}
		}

		private void EnsureInitialState()
		{
			if (isInitialized == false)
			{
				isInitialized = true;

				prefabPicker = new PrefabPicker<ViewModelComponent>(prefabs);
			}
		}

		private ViewModelComponent SpawnItem(ViewModelComponent bestPrefab, Transform parent)
		{
			ViewModelComponent result;

			if (pool)
			{
				result = pool.Spawn(bestPrefab, parent);
			}
			else
			{
				result = Instantiate(bestPrefab, parent);
			}

			return result;
		}
	}
}
