using System;

namespace Uice
{
	public interface IViewModelInjector
	{
		Type InjectionType { get; }
		ViewModelComponent Target { get; }
	}
}
