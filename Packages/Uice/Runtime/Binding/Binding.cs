using System;
using Uice.Utils;
using UnityEngine;

namespace Uice
{
    public abstract class Binding : IBinding
    {
        public abstract bool IsBound { get; }

        protected readonly BindingInfo bindingInfo;
        protected readonly Component viewModel;

        public Binding(BindingInfo bindingInfo, Component viewModel)
        {
            this.bindingInfo = bindingInfo;
            this.viewModel = viewModel;
        }

        public void Bind()
        {
            if (bindingInfo is ConstantBindingInfo constant && constant.UseConstant)
            {
                BindConstant(bindingInfo);
            }
            else
            {
                BindProperty();
            }
        }

        public void Unbind()
        {
            UnbindProperty();

            if (bindingInfo.ViewModelContainer)
            {
                bindingInfo.ViewModelContainer.ViewModelChanged -= OnViewModelChanged;
            }
        }

        protected abstract Type GetBindingType();

        protected abstract void BindProperty(object property);

        protected abstract void UnbindProperty();

        protected virtual void BindConstant(BindingInfo info)
        {
        }

        protected static string GetViewModelPath(Component context)
        {
            return $"{context.transform.PathToString()}@{context.GetType().Name}";
        }

        private void BindProperty()
        {
            if (HasToBeDynamicallyBound())
            {
                bindingInfo.ViewModelContainer = ViewModelComponentTree.FindBindableComponent(bindingInfo.Path, GetBindingType(), viewModel.transform);
            }

            if (bindingInfo.ViewModelContainer)
            {
                if (bindingInfo.ViewModelContainer.ViewModel != null)
                {
                    object value = ViewModelComponentTree.Bind(bindingInfo.Path, bindingInfo.ViewModelContainer);

                    if (value != null)
                    {
                        BindProperty(value);
                    }
                    else
                    {
                        Debug.LogError($"Property \"{bindingInfo.Path}\" not found in {bindingInfo.ViewModelContainer.ViewModel.GetType()} class.", viewModel);
                    }
                }

                bindingInfo.ViewModelContainer.ViewModelChanged += OnViewModelChanged;
            }
        }

        private bool HasToBeDynamicallyBound()
        {
            return (bindingInfo.ForceDynamicBinding || !bindingInfo.ViewModelContainer) && !string.IsNullOrEmpty(bindingInfo.Path);
        }

        private void OnViewModelChanged(object sender, IViewModel lastViewModel, IViewModel newViewModel)
        {
            Unbind();
            Bind();
        }
    }
}