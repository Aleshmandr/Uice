using System.Threading.Tasks;
using UnityEngine;

namespace Uice
{
    public static class Transition
    {
        private class NullTransition : ITransition
        {
            public void Prepare(Transform target) { }

            public Task Animate(Transform target)
            {
                return Task.CompletedTask;
            }

            public void Cleanup(Transform target) { }
        }

        public static readonly ITransition Null = new NullTransition();
    }
}
