using System;
using System.Globalization;

namespace Uice
{
	public class DateTimeToStringOperator : ToOperator<DateTime, string>
	{
		protected override string Convert(DateTime value)
		{
			return value.ToString(CultureInfo.CurrentCulture);
		}
	}
}