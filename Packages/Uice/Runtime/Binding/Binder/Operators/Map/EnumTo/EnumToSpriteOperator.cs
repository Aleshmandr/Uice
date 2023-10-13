using System;
using Uice.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Uice
{
    public class EnumToSpriteOperator<T> : MapOperator<T, Sprite> where T : Enum
    {
    }
}
