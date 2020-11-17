using System;

namespace LeastRecentCache
{
    public interface ICache<TM, T>
    {
        T GetData(TM request); //Retrieves data from cache or if not present from provider
        void ClearCache(); //Removes all the data from cache
        void ResizeCache(int size); //Changes the size of the cache
        int GetCacheFill(); //Returns number of elements in the cache

    }

    public abstract class CacheInitialise<TM, T> //abstract class that the cache will inherit from to ensure at least one constructor accepting provider is implemented
    {
        public CacheInitialise(IDataProvider<TM, T> provider)
        {
        }

        public CacheInitialise(IDataProvider<TM, T> provider, ILogger logger)
        {
        }
    }
}