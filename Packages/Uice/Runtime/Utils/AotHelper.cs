using System;
using Uice.Tweening;
using UnityEngine.Scripting;

namespace Uice
{
    public static class AotHelper
    {
        [Preserve]
        public static void EnsureBasicTypes()
        {
            Mace.AotHelper.EnsureType<Ease>();
            throw new InvalidOperationException("This method is used for AOT code generation only. Do not call it at runtime.");
        }
    }
}