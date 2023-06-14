using UnityEngine;

namespace Uice
{
	public class ObjectCollectionBatcherOperator : CollectionBatcherOperator<object>
	{
		protected override BindingInfo Collection => collection;

		[SerializeField] private BindingInfo collection = BindingInfo.Collection<object>();
	}
}
