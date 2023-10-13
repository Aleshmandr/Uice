using System;
using UnityEngine;

namespace Uice.Examples
{
	public class MyWindow : Window<MyContext>
	{
		private void Update()
		{
			Context.Timer.Value += Time.deltaTime;
		}
	}
}

