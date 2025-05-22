﻿using System.Threading.Tasks;
using UnityEngine;

namespace Uice
{
    public class AnimatorTransition : ComponentTransition
    {
        [SerializeField] private string stateName;
        [SerializeField] private bool rebindOnPlay;

        protected override void PrepareInternal(Transform target) { }

        protected override async Task AnimateInternal(Transform target)
        {
            if (target.TryGetComponent(out Animator animator))
            {
                animator.Play(stateName);
                
                if (rebindOnPlay)
                {
                    await Task.Yield();
                     
                    if (animator == null || !animator.isActiveAndEnabled)
                    {
                        return;
                    }
                     
                    animator.Rebind();
                }

                while (true)
                {
                    await Task.Yield();
                    
                    if (animator == null || !animator.isActiveAndEnabled)
                    {
                        return;
                    }
                    
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

        protected override void CleanupInternal(Transform target) { }
    }
}