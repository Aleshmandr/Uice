using System;

namespace Uice
{
	public interface IViewHideSettings
	{
		Type ViewType { get; }
		ITransition HideTransition { get; }
		ITransition ShowTransition { get; }
	}
}
