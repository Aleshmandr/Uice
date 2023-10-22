using System;

namespace Uice
{
	public interface IViewShowSettings
	{
		Type ViewType { get; }
		IViewModel ViewModel { get; }
		ITransition ShowTransition { get; }
		ITransition HideTransition { get; }
	}
}
