namespace Uice.Pooling
{
	public interface IPoolable
	{
		void OnSpawn();
		void OnRecycle();
	}
}
