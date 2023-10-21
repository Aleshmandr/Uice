﻿using System;
using UnityEngine;

namespace Uice
{
	public abstract class StartWithOperator<T> : Operator
	{
		[SerializeField] private ConstantBindingInfo<T> initialValue = new ConstantBindingInfo<T>();
		[SerializeField] private BindingInfo source = BindingInfo.Variable<T>();

		private VariableBinding<T> initialBinding;
		private ObservableVariable<T> exposedProperty;

		protected override void Awake()
		{
			base.Awake();

			RegisterVariable<T>(source)
				.OnChanged(OnBoundPropertyChanged)
				.OnCleared(OnBoundPropertyCleared);

			initialBinding = new VariableBinding<T>(initialValue, this);
			
			exposedProperty = new ObservableVariable<T>();
			Context = new OperatorVariableContext<T>(exposedProperty);
		}

		protected override void OnEnable()
		{
			initialBinding.Bind();

			if (initialBinding.Property.HasValue)
			{
				exposedProperty.Value = initialBinding.Property.Value;
			}

			base.OnEnable();
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			initialBinding.Unbind();
		}

		protected override Type GetInjectionType()
		{
			return typeof(OperatorVariableContext<T>);
		}

		private void OnBoundPropertyChanged(T newValue)
		{
			exposedProperty.Value = newValue;
		}

		private void OnBoundPropertyCleared()
		{
			exposedProperty.Clear();
		}
	}
}