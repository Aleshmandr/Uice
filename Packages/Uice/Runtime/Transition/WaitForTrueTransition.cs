using System.Threading.Tasks;
using Mace;
using UnityEngine;

namespace Uice
{
    public class WaitForTrueTransition : ComponentTransition
    {
        [SerializeField] private BindingInfo valueToCheck = BindingInfo.Variable<bool>();

        private VariableBinding<bool> valueToCheckBinding;

        protected override void Awake()
        {
            base.Awake();
            valueToCheckBinding = RegisterVariable<bool>(valueToCheck).GetBinding();
        }

        protected override void PrepareInternal(Transform target) { }

        protected override async Task AnimateInternal(Transform target)
        {
            while (valueToCheckBinding.IsBound == false || valueToCheckBinding.Property.GetValue(false) == false)
            {
                await Task.Yield();
            }
        }

        protected override void CleanupInternal(Transform target) { }
    }
}
