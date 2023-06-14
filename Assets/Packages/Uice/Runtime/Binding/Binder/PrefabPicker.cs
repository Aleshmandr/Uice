using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Uice
{
	public class PrefabPicker<T> where T : ContextComponent
	{
		private static readonly Type ExpectedContextType = typeof(IBindableContext<>);

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
			prefabs?.ForEach(x => Assert.IsNotNull(GetContextType(x.ExpectedType), $"{x.name}'s expected type is not valid. It must derive {ExpectedContextType}."));

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
				Type expectedType = prefab.ExpectedType;
				Type contextType;

				if (expectedType != null
					&& (contextType = GetContextType(expectedType)) != null
					&& contextType.GenericTypeArguments[0].IsAssignableFrom(valueType))
				{
					Type dataType = contextType.GenericTypeArguments[0];
					Type baseType = dataType.BaseType;
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

		private Type GetContextType(Type runtimeType)
		{
			Type result = null;

			Type genericType = null;

			if (runtimeType.IsGenericType)
			{
				genericType = runtimeType.GetGenericTypeDefinition();
			}

			if (genericType != null && genericType == ExpectedContextType)
			{
				result = runtimeType;
			}
			else
			{
				foreach (Type current in runtimeType.GetInterfaces())
				{
					result = GetContextType(current);

					if (result != null)
					{
						break;
					}
				}

				if (result == null && runtimeType.BaseType != null)
				{
					result = GetContextType(runtimeType.BaseType);
				}
			}

			return result;
		}
	}
}
