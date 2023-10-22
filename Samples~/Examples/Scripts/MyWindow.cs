using System;
using UnityEngine;

namespace Uice.Examples
{
	public class MyWindow : Window<MyViewModel>
	{
		private void Update()
		{
			ViewModel.Timer.Value += Time.deltaTime;
		}
	}
}

