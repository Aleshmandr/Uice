using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Uice
{
	public class PrefabPicker<T> where T : ViewModelComponent
	{
		private static readonly Type ExpectedViewModelType = typeof(IViewModel);

		private List<T> prefabs;
		private readonly Dictionary<Type, T> prefabResolutionCache;

		public PrefabPicker()
		{
			prefabs = new List<T>();
			prefabResolutionCache = new Dictionary<Type, T>();
		}

		public PrefabPicker(List<T> prefabs) : this()
		{
			SetPrefabs(prefabs);
		}

		public void SetPrefabs(List<T> prefabs)
		{
			prefabs?.ForEach(x => Assert.IsNotNull(x.ExpectedType, $"{x.name}'s expected type is not valid. It must derive {ExpectedViewModelType}."));

			this.prefabs = prefabs;
			prefabResolutionCache.Clear();
		}

		public T FindBestPrefab(object value)
		{
			Type valueType = value.GetType();

			if (prefabResolutionCache.TryGetValue(valueType, out var result) == false)
			{
				result = FindBestPrefab(valueType);

				if (result != null)
				{
					prefabResolutionCache[valueType] = result;
				}
			}

			return result;
		}

		private T FindBestPrefab(Type valueType)
		{
			T result = null;
			int bestDepth = -1;

			foreach (T prefab in prefabs)
			{
				Type prefabExpectedType = prefab.ExpectedType;

				if (prefabExpectedType != null && prefabExpectedType.IsAssignableFrom(valueType))
				{
					Type baseType = prefabExpectedType.BaseType;
					int depth = 0;

					while (baseType != null)
					{
						depth++;
						baseType = baseType.BaseType;
					}

					if (depth > bestDepth)
					{
						bestDepth = depth;
						result = prefab;
					}
				}
			}

			return result;
		}
	}
}
