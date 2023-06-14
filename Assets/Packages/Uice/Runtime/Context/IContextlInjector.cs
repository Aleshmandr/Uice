using System;

namespace Uice
{
	public interface IContextInjector
	{
		Type InjectionType { get; }
		ContextComponent Target { get; }
	}
}
