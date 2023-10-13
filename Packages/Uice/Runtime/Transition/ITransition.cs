using System.Threading.Tasks;
using UnityEngine;

namespace Uice
{
	public interface ITransition
	{
		void Prepare(Transform target);
		Task Animate(Transform target);
		void Cleanup(Transform target);
	}
}