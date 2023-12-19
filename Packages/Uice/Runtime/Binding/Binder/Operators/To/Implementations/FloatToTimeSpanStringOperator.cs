using System;
using UnityEngine;

namespace Uice
{
    public class FloatToTimeSpanStringOperator : ToOperator<float, string>
    {
        [SerializeField] private string format = "{0:mm\\:ss\\:ff}"; 
		
        protected override string Convert(float value)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(value);
            return string.Format(format, timeSpan);
        }
    }
}