﻿using UnityEngine;

namespace Juice
{
    public class TransformDecorateCommandOperator : DecorateCommandOperator<Transform>
    {
        protected override ConstantBindingInfo<Transform> DecorationBindingInfo => decorationBindingInfo;

        [SerializeField] private ConstantBindingInfo<Transform> decorationBindingInfo = new ConstantBindingInfo<Transform>();
    }
}
