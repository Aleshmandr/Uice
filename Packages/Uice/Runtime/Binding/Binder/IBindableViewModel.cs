﻿namespace Uice
{
	public interface IBindableViewModel<in T>
	{
		void Set(T value);
	}
}
