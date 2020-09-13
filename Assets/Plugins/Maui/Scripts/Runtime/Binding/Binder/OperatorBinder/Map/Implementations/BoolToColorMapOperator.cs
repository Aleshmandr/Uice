﻿using Maui.Collections;
using UnityEngine;

namespace Maui
{
	public class BoolToColorMapOperator : MapOperator<bool, Color>
	{
		protected override SerializableDictionary<bool, Color> Mapper => mapper;
		protected override ConstantBindingInfo Fallback => fallback;

		[SerializeField] private BoolColorDictionary mapper;
		[SerializeField] private ColorConstantBindingInfo fallback;
		
	}
}