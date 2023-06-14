using System;

namespace Uice
{
    public abstract class EnumToBoolOperator<T> : MapOperator<T, bool> where T : Enum
    {
    }
}
