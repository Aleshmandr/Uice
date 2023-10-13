using System;
using System.Threading.Tasks;

namespace Uice
{
	public interface IWindowShowLauncher
	{
		IWindowShowLauncher WithContext(IContext context);
		IWindowShowLauncher AddPayload(string key, object value);
		IWindowShowLauncher WithShowTransition(ITransition transition);
		IWindowShowLauncher WithHideTransition(ITransition transition);
		IWindowShowLauncher WithPriority(WindowPriority priority);
		IWindowShowLauncher WithBackDestination(Type viewType);
		IWindowShowLauncher WithStubViewType(Type viewType);
		void Execute();
		Task ExecuteAsync();
		void InForeground();
		Task InForegroundAsync();
		void Enqueue();
	}
}
