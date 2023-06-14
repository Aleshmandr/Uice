namespace Uice
{
	public interface IBindableContext<in T>
	{
		void Set(T value);
	}
}
