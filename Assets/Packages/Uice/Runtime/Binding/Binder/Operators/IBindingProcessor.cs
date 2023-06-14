namespace Uice
{
	public interface IBindingProcessor
	{
		IContext Context { get; }

		void Bind();
		void Unbind();
	}
}