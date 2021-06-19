﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Juice.Plugins.Juice.Runtime.Utils;
using Juice.Plugins.Juice.Runtime.Utils.ExtensionMethods;
using UnityEngine;

namespace Juice
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

		protected static string GetContextPath(Component context)
		{
			return $"{context.transform.PathToString()}@{context.GetType().Name}";
		}

		private void BindProperty()
		{
			if (HasToBeDynamicallyBound())
			{
				bindingInfo.ViewModelContainer = ViewModelComponentTree.FindBindableComponent(
					bindingInfo.Path,
					GetBindingType(),
					context.transform);
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
						Debug.LogError($"Property \"{bindingInfo.Path.PropertyName}\" not found in {bindingInfo.ViewModelContainer.ViewModel.GetType()} class.", context);
					}
				}

				bindingInfo.ViewModelContainer.ViewModelChanged += OnViewModelChanged;
			}
		}

		private static ViewModelComponent FindViewModelComponent(Transform context, Type targetType, string propertyPath)
		{
			ViewModelComponent result = null;

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
						result = iterator.Current.ViewModelComponent;
					}
				}
			}

			return result;
		}

		private bool HasToBeDynamicallyBound()
		{
			return bindingInfo.ForceDynamicBinding || !bindingInfo.ViewModelContainer && string.IsNullOrEmpty(bindingInfo.PropertyName) == false;
		}

		private void OnViewModelChanged(object sender, IViewModel lastViewModel, IViewModel newViewModel)
		{
			Unbind();
			Bind();
		}
	}
}
