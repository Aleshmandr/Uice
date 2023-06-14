using System;
using UnityEngine;

namespace Uice
{
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(ContextComponent))]
	[RequireComponent(typeof(InteractionBlockingTracker))]
	public abstract class View<T> : Widget, IView, IContextInjector, IContextProvider<T> where T : IContext
	{
		public event ViewEventHandler CloseRequested;
		public event ViewEventHandler ViewDestroyed;
		public event ContextChangeEventHandler<T> ContextChanged;

		public bool IsInteractable
		{
			get
			{
				EnsureInitialState();
				return blockingTracker.IsInteractable;
			}

			set
			{
				EnsureInitialState();
				blockingTracker.IsInteractable = value;
			}
		}

		public Type InjectionType => typeof(T);
		public ContextComponent Target => targetComponent;
		public T Context => (T)(Target ? Target.Context : default);

		[SerializeField, HideInInspector] private ContextComponent targetComponent;
		[SerializeField, HideInInspector] private InteractionBlockingTracker blockingTracker;

		protected virtual void Reset()
		{
			RetrieveRequiredComponents();
		}

		protected virtual void OnDestroy()
		{
			ViewDestroyed?.Invoke(this);
		}

		public virtual void SetContext(IContext context)
		{
			if (context != null)
			{
				if (context is T typedContext)
				{
					SetContext(typedContext);
				}
				else
				{
					Debug.LogError($"Context passed have wrong type! ({context.GetType()} instead of {typeof(T)})", this);
				}
			}
		}

		public void Close()
		{
			CloseRequested?.Invoke(this);
		}

		protected override void Initialize()
		{
			base.Initialize();

			RetrieveRequiredComponents();

			Target.ContextChanged += OnTargetComponentContextChanged;
		}

		protected virtual void SetContext(T context)
		{
			EnsureInitialState();

			if (targetComponent)
			{
				targetComponent.Context = context;
			}
		}

		protected virtual void OnContextChanged(T lastContext, T newContext)
		{
			ContextChanged?.Invoke(this, lastContext, newContext);
		}

		private void RetrieveRequiredComponents()
		{
			if (!targetComponent)
			{
				targetComponent = GetComponent<ContextComponent>();
			}

			if (!blockingTracker)
			{
				blockingTracker = GetComponent<InteractionBlockingTracker>();
			}
		}

		private void OnTargetComponentContextChanged(IContextProvider<IContext> source, IContext lastContext, IContext newContext)
		{
			OnContextChanged((T)lastContext, (T)newContext);
		}
	}
}
