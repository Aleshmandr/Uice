using System;
using System.Collections.Generic;
using Uice.Utils;
using Uice.Pooling;
using UnityEngine;

namespace Uice
{
	public class SpawnPrefabBinder: ComponentBinder
	{
		private Transform Parent => parent ? parent : transform;

		[SerializeField] private BindingInfo objectToInstantiate = BindingInfo.Variable<object>();
		[SerializeField] private List<BindableContextComponent> prefabs;
		[SerializeField] private Transform parent;
		[SerializeField] private ObjectPool pool;

		private PrefabPicker<BindableContextComponent> prefabPicker;
		private BindableContextComponent currentItem;

		protected void Reset()
		{
			pool = GetComponent<ObjectPool>();
		}

		protected virtual void OnValidate()
		{
			if (!parent)
			{
				parent = transform;
			}
		}

		protected override void Awake()
		{
			base.Awake();

			RegisterVariable<object>(objectToInstantiate)
				.OnChanged(OnObjectChanged)
				.OnCleared(OnObjectCleared);

			prefabPicker = new PrefabPicker<BindableContextComponent>(prefabs);
			currentItem = null;

			if (!pool)
			{
				pool = this.GetOrAddComponent<ObjectPool>();
			}

			foreach (BindableContextComponent prefab in prefabs)
			{
				pool.CreatePool(prefab, 1);
			}
		}

		private void OnObjectChanged(object value)
		{
			BindableContextComponent bestPrefab = prefabPicker.FindBestPrefab(value);

			if (bestPrefab)
			{
				if (currentItem == null || bestPrefab.ExpectedType != currentItem.ExpectedType)
				{
					currentItem = SpawnItem(bestPrefab, Parent);
				}

				currentItem.SetData(value);
			}
			else
			{
				Clear();

				Debug.LogError($"A matching prefab could not be found for {value} ({value.GetType().GetPrettifiedName()})");
			}
		}

		private void OnObjectCleared()
		{
			Clear();
		}

		private BindableContextComponent SpawnItem(BindableContextComponent prefab, Transform parent)
		{
			Clear();

			return pool.Spawn(prefab, parent, false);
		}

		private void Clear()
		{
			if (currentItem != null)
			{
				pool.Recycle(currentItem);
				currentItem = null;
			}
		}
	}
}
