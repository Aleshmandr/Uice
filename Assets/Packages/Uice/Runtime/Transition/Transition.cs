using System.Threading.Tasks;
using UnityEngine;

namespace Uice
{
	public static class Transition
	{
		private class NullTransition : ITransition
		{
			public void Prepare(RectTransform target)
			{

			}

			public Task Animate(RectTransform target)
			{
				return Task.CompletedTask;
			}

			public void Cleanup(RectTransform target)
			{

			}
		}

		public static readonly ITransition Null = new NullTransition();
	}
}
