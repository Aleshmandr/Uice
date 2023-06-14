using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Uice
{
	public abstract class ContextComponent<T> : MonoBehaviour, IContextProvider<T> where T : IContext
	{
		public event ContextChangeEventHandler<T> ContextChanged;

		public virtual Type ExpectedType => expectedType.Type;

		public T Context
		{
			get => context;
			set
			{
				T contextModel = context;
				context?.Disable();
				context = value;
				context?.Enable();
				OnContextChanged(contextModel, context);
			}
		}

		public string Id => id;

		[TypeConstraint(typeof(IContext))]
		[SerializeField, HideInInspector] protected SerializableType expectedType;
		[SerializeField, DisableAtRuntime] private string id;

		private T context;

		protected virtual void Reset()
		{
			ResetId();
		}

		protected virtual void OnValidate()
		{
			EnsureId();
		}

		protected virtual void Awake()
		{
			EnsureId();
		}

		protected virtual void OnContextChanged(T lastContext, T newContext)
		{
			ContextChanged?.Invoke(this, lastContext, newContext);
		}
		
		private void EnsureId()
		{
			if (string.IsNullOrEmpty(id) || IsIdAvailable(id) == false)
			{
				ResetId();
			}
		}

		private void ResetId()
		{
			string candidate = GetNewId();

			while (IsIdAvailable(candidate) == false)
			{
				candidate = GetNewId();
			}

			id = candidate;
		}

		private string GetNewId()
		{
			return Guid.NewGuid().ToString().Substring(0, 4);
		}

		private bool IsIdAvailable(string candidate)
		{
			IEnumerable<ContextComponent<IContext>> components =
				GetComponentsInChildren<Component>(true)
					.Concat(GetComponentsInParent<Component>())
					.Where(x => x is ContextComponent<IContext>)
					.Cast<ContextComponent<IContext>>();
			bool result = IsIdAvailable(candidate, components);
			return result;
		}

		private bool IsIdAvailable(string candidate, IEnumerable<ContextComponent<IContext>> components)
		{
			var existingId = components.FirstOrDefault(x => x != this && string.Equals(candidate, x.id));

			if (existingId)
			{
				Debug.LogError($"Id \"{candidate}\" already taken by {existingId.name}", this);
			}

			return !existingId;
		}
	}

	public class ContextComponent : ContextComponent<IContext>
	{
		protected override void Awake()
		{
			base.Awake();
			
			ContextComponentTree.Register(this);
		}

		protected virtual void OnDestroy()
		{
			ContextComponentTree.Unregister(this);
		}

		protected virtual void OnTransformParentChanged()
		{
			ContextComponentTree.Move(this);
		}
	}
}
