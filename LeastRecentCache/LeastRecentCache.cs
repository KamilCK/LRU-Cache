using System;
using System.Collections.Generic;

namespace LeastRecentCache
{
    public sealed class LeastRecentCache<TM,T> : CacheInitialise<TM, T>, ICache<TM, T>
    {
        private int _size = 10;
        private static LeastRecentCache<TM,T> _instance;
        private IDataProvider<TM,T> _dataProvider;
        private ILogger _logger;
        private OrderPreservingDictionary<TM,T> _data = new OrderPreservingDictionary<TM, T>();

        private static readonly object CacheLock = new object();

        public LeastRecentCache(IDataProvider<TM, T> provider) : base(provider)
        {
            _dataProvider = provider;
        }

        public LeastRecentCache(IDataProvider<TM, T> provider, ILogger logger) : base(provider, logger)
        {
            _dataProvider = provider;
        }


        public void RegisterLogger(ILogger logger)
        {
            //Requires a thread safe logger
            lock (_logger)
            {
                _logger = logger;
            }
        }

        public T GetData(TM request)
        {
            T value;
            lock (_data)
            {
                if (_data.TryGetValue(request, out value))
                {
                    Log("Data retrieved from cache: " + request);
                    _data.Reinsert(request); 
                    Log("Key reinserted: " + request);
                    return value;
                }
            }

            value = _dataProvider.GetData(request);
            lock (_data)
            {
                Log("Data retrieved from provider: " + request);
                _data.Remove(request); //in case another thread added it during retrieval
                Log("Key inserted: " + request);
                _data.Add(request, value);

                if (_data.Count > _size)
                {
                    _data.RemoveFirst();
                    Log("Capacity reached, key removed");
                }

                return value;
            }   

        }

        public List<TM> GetCacheKeys()
        {
            lock (_data)
            {
                return _data.GetKeys();
            }
        }
        public void ClearCache()
        {
            lock (_data)
            {
                _data = new OrderPreservingDictionary<TM, T>();
            }
        }

        public void ResizeCache(int size)
        {
            lock (_data)
            {
                _size = Math.Max(0,size);

                while (_size < _data.Count)
                {
                    _data.RemoveFirst();
                }
            }
        }

        public int GetCacheFill()
        {
            lock (_data)
            {
                return _data.Count;
            }
        }

        private void Log(string message)
        {
            _logger?.Log(message);
        }

    }


}