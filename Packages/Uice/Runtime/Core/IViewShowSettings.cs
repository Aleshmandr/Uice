using System;

namespace Uice
{
	public interface IViewShowSettings
	{
		Type ViewType { get; }
		IContext Context { get; }
		ITransition ShowTransition { get; }
		ITransition HideTransition { get; }
	}
}
