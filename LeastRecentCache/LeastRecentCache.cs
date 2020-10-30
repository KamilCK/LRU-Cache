using System;
using System.Collections.Generic;

namespace LeastRecentCache
{
    public class LeastRecentCache<TM,T> : ICache<TM,T>
    {
        private static LeastRecentCache<TM,T> _instance;
        private int _size=1;
        private IDataProvider<TM,T> _dataProvider;
        private ILogger _logger;

        private OrderPreservingDictionary<TM,T> _data = new OrderPreservingDictionary<TM, T>();

        private static readonly object cacheLock = new object();

        private LeastRecentCache()
        {

        }

        public static LeastRecentCache<TM,T> Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (cacheLock)
                    {
                        _instance ??= new LeastRecentCache<TM, T>();
                    }
                }
                return _instance;
            }
        }

        public void RegisterDataProvider(IDataProvider<TM, T> provider)
        {
            ClearCache();
            _dataProvider = provider;
        }

        public void RegisterLogger(ILogger logger)
        {
            _logger = logger;
        }

        public T GetData(TM request)
        {
            lock (_data)
            {
                if (_data.TryGetValue(request, out var value))
                {
                    if (!request.Equals(_data.GetLastKey()))
                    {
                        Log("Data retrieved from cache: " + request);
                            _data.Reinsert(request);
                            Log("Key reinserted: " + request);
                    }
                }
                else
                {
                    value = _dataProvider.GetData(request);
                    Log("Data retrieved from provider: " + request);
                    if (_data.ContainsKey(request))
                        {
                            _data.Remove(request);
                        }

                        Log("Key inserted: " + request);
                        _data.Add(request, value);

                        if (_data.Count > _size)
                        {
                            _data.RemoveFirst();
                            Log("Capacity reached, key removed");
                        }

                }
                return value;
            }

        }

        public List<TM> GetCacheKeys()
        {
            return _data.GetKeys();
        }
        public void ClearCache()
        {
            _data = new OrderPreservingDictionary<TM, T>();
        }

        public void ResizeCache(int size)
        {
            _size = size;

            while (_size < _data.Count)
            {
                _data.RemoveFirst();
            }
        }

        public int GetCacheFill()
        {
            return _data.Count;
        }

        private void Log(string message)
        {
            _logger?.Log(message);
        }

    }


}