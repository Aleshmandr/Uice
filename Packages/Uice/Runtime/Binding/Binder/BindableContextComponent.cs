using System;
using Uice.Utils;
using UnityEngine;

namespace Uice
{
    public class BindableContextComponent : ContextComponent, IContextInjector
    {
        [TypeConstraint(typeof(IContext), true)]
        [SerializeField] protected SerializableType expectedContextType = new();
        [SerializeField] private BindingInfo bindingInfo;
        private VariableBinding<object> binding;
        
        public Type InjectionType => expectedContextType.Type;
        public override Type ExpectedType => expectedContextType.Type;
        public ContextComponent Target => this;

        protected override void Awake()
        {
            base.Awake();

            binding = new VariableBinding<object>(bindingInfo, this);
            binding.Property.Changed += SetData;
        }

        protected virtual void OnEnable()
        {
            binding.Bind();
        }

        protected virtual void OnDisable()
        {
            binding.Unbind();
        }

        public void SetData(object data)
        {
            if (Context == null)
            {
                if (ExpectedType != null)
                {
                    Context = (IContext)data;
                } else
                {
                    Debug.LogError("Expected Type must be set", this);
                }
            }
        }
        
        protected override void OnValidate()
        {
            base.OnValidate();

            if (!Application.isPlaying && expectedContextType != null && expectedContextType.Type != null)
            {
                Type dataType = ExpectedType;
                Type bindingType = typeof(IReadOnlyObservableVariable<>).MakeGenericType(dataType);

                if (bindingInfo.Type != bindingType)
                {
                    bindingInfo = new BindingInfo(bindingType);
                    BindingInfoTrackerProxy.RefreshBindingInfo();
                }
            }
        }
    }
}
