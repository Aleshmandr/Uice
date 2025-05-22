using System.Threading.Tasks;
using Uice;
using UnityEngine;

namespace Uice
{
    public class EmptyTransition : ITransition
    {
        public static EmptyTransition Transition = new EmptyTransition();
        
        public void Prepare(Transform target) { }

        public Task Animate(Transform target)
        {
            return Task.CompletedTask;
        }

        public void Cleanup(Transform target) { }
    }
}