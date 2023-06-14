using Uice.Utils;

namespace Uice.Pooling
{
	public class SingleObjectPool : Singleton<SingleObjectPool>
	{
		public ObjectPool GlobalPool
		{
			get
			{
				if (!pool)
				{
					pool = this.GetOrAddComponent<ObjectPool>();
				}

				return pool;
			}

		}

		private ObjectPool pool;
	}
}
