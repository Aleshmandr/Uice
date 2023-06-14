using System;
using System.Collections.Generic;
using System.Reflection;
using Uice.Plugins.Juice.Runtime.Utils.ExtensionMethods;
using Uice.Plugins.Juice.Runtime.Utils;
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
				bindingInfo.ContextContainer = ContextComponentTree.FindBindableComponent(
					bindingInfo.Path,
					GetBindingType(),
					context.transform);
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
						Debug.LogError($"Property \"{bindingInfo.Path.PropertyName}\" not found in {bindingInfo.ContextContainer.Context.GetType()} class.", context);
					}
				}

				bindingInfo.ContextContainer.ContextChanged += OnContextChanged;
			}
		}

		private static ContextComponent FindContextComponent(Transform context, Type targetType, string propertyPath)
		{
			ContextComponent result = null;

			BindingPath path = new BindingPath(propertyPath);

			using (IEnumerator<BindingEntry> iterator = BindingUtils.GetBindings(context, targetType).GetEnumerator())
			{
				while (!result && iterator.MoveNext())
				{
					BindingEntry current = iterator.Current;

					bool match = string.IsNullOrEmpty(path.ComponentId) || path.ComponentId == current.Path.ComponentId;
					match &= path.PropertyName == current.Path.PropertyName;

					if (match)
					{
						result = iterator.Current.ContextComponent;
					}
				}
			}

			return result;
		}

		private bool HasToBeDynamicallyBound()
		{
			return bindingInfo.ForceDynamicBinding || !bindingInfo.ContextContainer && string.IsNullOrEmpty(bindingInfo.PropertyName) == false;
		}

		private void OnContextChanged(object sender, IContext lastContext, IContext newContext)
		{
			Unbind();
			Bind();
		}
	}
}
