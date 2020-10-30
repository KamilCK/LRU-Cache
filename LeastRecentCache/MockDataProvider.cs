
using System.Collections.Generic;


namespace LeastRecentCache
{
    public class MockDataProvider<TM,T> : IDataProvider<TM,T>
    {
        private readonly Dictionary<TM, T> _dataDict;

        public MockDataProvider(Dictionary<TM, T> dict )
        {
            _dataDict = dict;
        }

        public T GetData(TM key)
        {
            if (_dataDict.TryGetValue(key, out var value))
            {
                return value;
            }
            else
            {
                return default;
            }
        }

    }
}
