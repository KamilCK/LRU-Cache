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

        private static readonly object CacheLock = new object();

        private LeastRecentCache()
        {

        }

        public static LeastRecentCache<TM,T> Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (CacheLock)
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
            T value;
            lock (_data)
            {
                if (_data.TryGetValue(request, out value))
                {
                    Log("Data retrieved from cache: " + request);
                    if (!request.Equals(_data.GetLastKey()))
                    {
                        _data.Reinsert(request);
                        Log("Key reinserted: " + request);
                    }
                    return value;
                }
            }

            value = _dataProvider.GetData(request);
            lock (_data)
            {
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