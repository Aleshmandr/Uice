using System.Threading.Tasks;
using UnityEngine;

namespace Uice
{
    public class AnimatorTransition : ComponentTransition
    {
        [SerializeField] private string stateName;
        
        protected override void PrepareInternal(Transform target)
        {
            
        }

        protected override async Task AnimateInternal(Transform target)
        {
            if (target.TryGetComponent(out Animator animator))
            {
                animator.Play(stateName);

                while (true)
                {
                    await Task.Yield();
                    var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

                    if (!stateInfo.IsName(stateName))
                    {
                        continue;
                    }

                    if (stateInfo.normalizedTime >= 1f)
                    {
                        break;
                    }
                }
            }
        }

        protected override void CleanupInternal(Transform target)
        {
            
        }
    }
}