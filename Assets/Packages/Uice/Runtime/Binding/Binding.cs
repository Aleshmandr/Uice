using System;
using Uice.Utils;
using UnityEngine;

namespace Uice
{
	public abstract class Binding : IBinding
	{
		public abstract bool IsBound { get; }

		protected readonly BindingInfo bindingInfo;
		protected readonly Component context;

		public Binding(BindingInfo bindingInfo, Component context)
		{
			this.bindingInfo = bindingInfo;
			this.context = context;
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

			if (bindingInfo.ContextContainer)
			{
				bindingInfo.ContextContainer.ContextChanged -= OnContextChanged;
			}
		}

		protected abstract Type GetBindingType();

		protected abstract void BindProperty(object property);

		protected abstract void UnbindProperty();

		protected virtual void BindConstant(BindingInfo info)
		{

		}

		protected static string GetContextPath(Component context)
		{
			return $"{context.transform.PathToString()}@{context.GetType().Name}";
		}

		private void BindProperty()
		{
			if (HasToBeDynamicallyBound())
			{
				bindingInfo.ContextContainer = ContextComponentTree.FindBindableComponent(bindingInfo.Path, GetBindingType(), context.transform);
			}

			if (bindingInfo.ContextContainer)
			{
				if (bindingInfo.ContextContainer.Context != null)
				{
					object value = ContextComponentTree.Bind(bindingInfo.Path, bindingInfo.ContextContainer);

					if (value != null)
					{
						BindProperty(value);
					}
					else
					{
						Debug.LogError($"Property \"{bindingInfo.Path}\" not found in {bindingInfo.ContextContainer.Context.GetType()} class.", context);
					}
				}

				bindingInfo.ContextContainer.ContextChanged += OnContextChanged;
			}
		}

		private bool HasToBeDynamicallyBound()
		{
			return (bindingInfo.ForceDynamicBinding || !bindingInfo.ContextContainer) && !string.IsNullOrEmpty(bindingInfo.Path);
		}

		private void OnContextChanged(object sender, IContext lastContext, IContext newContext)
		{
			Unbind();
			Bind();
		}
	}
}
