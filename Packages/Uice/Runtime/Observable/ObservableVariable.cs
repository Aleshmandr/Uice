using System;
using System.Collections.Generic;

namespace Uice
{
	[Serializable]
	public class ObservableVariable<T> : IObservableVariable<T>
	{
		public event ObservableVariableEventHandler<T> Changed;
		public event ObservableVariableClearEventHandler Cleared;
		
		private T value;
		private EqualityComparer<T> equalityComparer;

		public bool HasValue { get; protected set; }

		public T Value
		{
			get => value;

			set
			{
				if (!HasValue || !Compare(value, this.value))
				{
					SetValue(value);
					OnChanged(value);
				}
			}
		}
		
		public ObservableVariable()
		{
			equalityComparer = EqualityComparer<T>.Default;
		}
		
		public ObservableVariable(T initialValue) : this()
		{
			SetValue(initialValue);
		}

		public ObservableVariable(EqualityComparer<T> equalityComparer) : this()
		{
			this.equalityComparer = equalityComparer;
		}
		
		public static implicit operator T(ObservableVariable<T> value)
		{
			return value.Value;
		}

		public void Clear()
		{
			value = default;
			HasValue = false;
			OnCleared();
		}
		
		public void ForceChangedNotification()
		{
			OnChanged(Value);
		}

		protected virtual void OnChanged(T newValue)
		{
			Changed?.Invoke(newValue);
		}

		protected virtual void OnCleared()
		{
			Cleared?.Invoke();
		}
		
		private bool Compare(T x, T y)
		{
			return equalityComparer.Equals(x, y);
		}

		private void SetValue(T newValue)
		{
			value = newValue;
			HasValue = true;
		}
	}
}
