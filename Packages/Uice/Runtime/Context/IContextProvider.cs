namespace Uice
{
    public delegate void ContextChangeEventHandler<T>(
        IContextProvider<T> source,
        T lastContext,
        T newContext)
        where T : IContext;

    public interface IContextProvider<T> where T : IContext
    {
        event ContextChangeEventHandler<T> ContextChanged;

        T Context { get; }
    }
}
