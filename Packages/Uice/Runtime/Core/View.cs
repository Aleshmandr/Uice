using System;
using Mace;
using UnityEngine;

namespace Uice
{
    [RequireComponent(typeof(ViewModelComponent))]
    [RequireComponent(typeof(InteractionBlockingTracker))]
    public abstract class View<T> : Widget, IView, IViewModelInjector, IViewModelProvider<T> where T : IViewModel
    {
        public event ViewEventHandler CloseRequested;
        public event ViewEventHandler ViewDestroyed;
        public event ViewModelChangeEventHandler<T> ViewModelChanged;

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
        public ViewModelComponent Target => targetComponent;
        public T ViewModel => (T)(Target ? Target.ViewModel : default);

        [SerializeField, HideInInspector] private ViewModelComponent targetComponent;
        [SerializeField, HideInInspector] private InteractionBlockingTracker blockingTracker;

        protected virtual void Reset()
        {
            RetrieveRequiredComponents();
        }

        protected virtual void OnDestroy()
        {
            ViewDestroyed?.Invoke(this);
        }

        public virtual void SetViewModel(IViewModel viewModel)
        {
            if (viewModel != null)
            {
                if (viewModel is T typedViewModel)
                {
                    SetViewModel(typedViewModel);
                }
                else
                {
                    Debug.LogError($"ViewModel passed have wrong type! ({viewModel.GetType()} instead of {typeof(T)})", this);
                }
            }
        }

        public void Close()
        {
            CloseRequested?.Invoke(this);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        protected override void Initialize()
        {
            base.Initialize();
            RetrieveRequiredComponents();
            Target.ViewModelChanged += OnTargetViewModelChanged;
        }

        protected virtual void SetViewModel(T context)
        {
            EnsureInitialState();

            if (targetComponent)
            {
                targetComponent.ViewModel = context;
            }
        }

        protected virtual void OnViewModelChanged(T lastViewModel, T newViewModel)
        {
            ViewModelChanged?.Invoke(this, lastViewModel, newViewModel);
        }

        private void RetrieveRequiredComponents()
        {
            if (!targetComponent)
            {
                targetComponent = GetComponent<ViewModelComponent>();
            }

            if (!blockingTracker)
            {
                blockingTracker = GetComponent<InteractionBlockingTracker>();
            }
        }

        private void OnTargetViewModelChanged(IViewModelProvider<IViewModel> source, IViewModel lastViewModel, IViewModel newViewModel)
        {
            OnViewModelChanged((T)lastViewModel, (T)newViewModel);
        }
    }
}