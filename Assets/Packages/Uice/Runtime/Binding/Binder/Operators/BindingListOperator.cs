using System;
using System.Collections.Generic;
using UnityEngine;

namespace Uice
{
	public abstract class BindingListOperator<TFrom, TTo> : ContextComponent, IContextInjector
	{
		public Type InjectionType => typeof(OperatorVariableContext<TTo>);

		public ContextComponent Target => this;

		[SerializeField] private BindingInfoList fromBinding = new BindingInfoList(typeof(IReadOnlyObservableVariable<TFrom>));

		private BindingList<TFrom> bindingList;
		private ObservableVariable<TTo> exposedProperty;

		protected override void Awake()
		{
			base.Awake();

			exposedProperty = new ObservableVariable<TTo>();
			Context = new OperatorVariableContext<TTo>(exposedProperty);

			bindingList = new BindingList<TFrom>(this, fromBinding);
			bindingList.VariableChanged += BindingListVariableChangedHandler;
		}

		protected virtual void OnEnable()
		{
			bindingList.Bind();
		}

		protected virtual void OnDisable()
		{
			bindingList.Unbind();
		}

		protected abstract TTo Process(int triggerIndex, IReadOnlyList<TFrom> list);

		private void BindingListVariableChangedHandler(int index, TFrom newValue)
		{
			exposedProperty.Value = Process(index, bindingList.Values);
		}
	}
}
