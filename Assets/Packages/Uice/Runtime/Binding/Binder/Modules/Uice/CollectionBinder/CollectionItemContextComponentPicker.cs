using System.Collections.Generic;
using Uice.Utils;
using Uice.Pooling;
using UnityEngine;
using UnityEngine.Assertions;

namespace Uice
{
	public class CollectionItemContextComponentPicker : ItemPicker
	{
		[SerializeField] private List<ContextComponent> prefabs;
		[SerializeField] private ObjectPool pool;

		private PrefabPicker<ContextComponent> prefabPicker;
		private bool isInitialized;

		protected virtual void Awake()
		{
			EnsureInitialState();
		}

		public override ContextComponent SpawnItem(int index, IContext value, Transform parent)
		{
			EnsureInitialState();

			ContextComponent bestPrefab = prefabPicker.FindBestPrefab(value);

			Assert.IsNotNull(bestPrefab, $"A suitable prefab could not be found for {value} ({value.GetType().GetPrettifiedName()}).");

			return SpawnItem(bestPrefab, parent);
		}

		public override ContextComponent ReplaceItem(int index, IContext oldValue, IContext newValue, ContextComponent currentItem, Transform parent)
		{
			EnsureInitialState();

			ContextComponent result = currentItem;

			var prefabForOldValue = prefabPicker.FindBestPrefab(oldValue);
			var prefabForNewValue = prefabPicker.FindBestPrefab(newValue);

			if (ReferenceEquals(prefabForOldValue, prefabForNewValue) == false)
			{
				result = SpawnItem(prefabForNewValue, parent);
			}

			return result;
		}

		public override void DisposeItem(int index, ContextComponent item)
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

				prefabPicker = new PrefabPicker<ContextComponent>(prefabs);
			}
		}

		private ContextComponent SpawnItem(ContextComponent bestPrefab, Transform parent)
		{
			ContextComponent result;

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
