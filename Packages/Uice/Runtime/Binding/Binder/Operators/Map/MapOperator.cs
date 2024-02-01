using Uice.Collections;
using UnityEngine;

namespace Uice
{
    public abstract class MapOperator<TFrom, TTo> : ToOperator<TFrom, TTo>
    {
        [SerializeField] private SerializableDictionary<TFrom, TTo> mapper = new SerializableDictionary<TFrom, TTo>();
        [SerializeField] private ConstantBindingInfo<TTo> fallback = new ConstantBindingInfo<TTo>();

        private SerializableDictionary<TFrom, TTo> Mapper => mapper;
        private ConstantBindingInfo Fallback => fallback;

        private VariableBinding<TTo> fallbackBinding;

        protected override void Awake()
        {
            base.Awake();
            fallbackBinding = new VariableBinding<TTo>(Fallback, this);
        }

        protected override void OnEnable()
        {
            fallbackBinding.Bind();
            fallbackBinding.Property.Changed += HandleFallbackPropertyChange;
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            fallbackBinding.Property.Changed -= HandleFallbackPropertyChange;
            fallbackBinding.Unbind();
        }
        
        private void HandleFallbackPropertyChange(TTo newValue)
        {
            if (bindingProcessor is ToVariableBindingProcessor<TFrom, TTo> variableBindingProcessor)
            {
                variableBindingProcessor.Refresh();
            }
        }

        protected override TTo Convert(TFrom value)
        {
            if (!Mapper.TryGetValue(value, out var result))
            {
                result = fallbackBinding.Property.Value;
            }

            return result;
        }
    }
}