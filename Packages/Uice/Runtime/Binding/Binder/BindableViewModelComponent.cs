using System;
using Uice.Utils;
using UnityEngine;

namespace Uice
{
    public class BindableViewModelComponent : ViewModelComponent, IViewModelInjector
    {
        [TypeConstraint(typeof(IViewModel), true)] [SerializeField]
        protected SerializableType expectedViewModelType = new();

        [SerializeField] private BindingInfo bindingInfo;
        private VariableBinding<object> binding;

        public Type InjectionType => expectedViewModelType.Type;
        public override Type ExpectedType => expectedViewModelType.Type;
        public ViewModelComponent Target => this;

        protected override void Awake()
        {
            base.Awake();
            binding = new VariableBinding<object>(bindingInfo, this);
        }

        protected virtual void OnEnable()
        {
            binding.Property.Changed += SetData;
            binding.Bind();
        }

        protected virtual void OnDisable()
        {
            binding.Property.Changed -= SetData;
            binding.Unbind();
        }

        public void SetData(object data)
        {
            if (ViewModel == null)
            {
                if (ExpectedType != null)
                {
                    ViewModel = (IViewModel)data;
                }
                else
                {
                    Debug.LogError("Expected Type must be set", this);
                }
            }
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (!Application.isPlaying && expectedViewModelType != null && expectedViewModelType.Type != null)
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