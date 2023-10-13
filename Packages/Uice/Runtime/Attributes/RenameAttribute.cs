using System;
using UnityEngine;

namespace Uice
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
	public class RenameAttribute : PropertyAttribute
	{
		public string PropertyLookupName { get; }

		public RenameAttribute(string propertyLookupName)
		{
			PropertyLookupName = propertyLookupName;
		}
	}
}
