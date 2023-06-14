using System;
using UnityEngine;

namespace Uice
{
    public abstract class EnumToColorOperator<T> : MapOperator<T, Color> where T : Enum
    {
    }
}
