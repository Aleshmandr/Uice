﻿using UnityEngine;

namespace Uice
{
	public class Vector3ToStringOperator : ToOperator<Vector3, string>
	{
		protected override string Convert(Vector3 value)
		{
			return value.ToString();
		}
	}
}