using System.Collections.Generic;

namespace Uice
{
	public interface IObservableDictionary<TKey, TValue> : IReadOnlyObservableDictionary<TKey, TValue>, IDictionary<TKey, TValue>
	{

	}
}
