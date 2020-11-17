
using System.Collections.Generic;

namespace LeastRecentCache
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = new Dictionary<int, int>() { { 1, 100 }, { 2, 200 }, { 3, 300 }, { 4, 400 } };
            MockDataProvider<int, int> provider = new MockDataProvider<int, int>(data);

            var cache = new LeastRecentCache<int, int>(provider);

            //LeastRecentCache<int, int>.Instance.RegisterDataProvider(provider);
            cache.ResizeCache(2);
            cache.RegisterLogger(new MockLogger());
            cache.GetData(1);
            cache.GetData(2);
            cache.GetData(2);
            cache.GetData(1);
            cache.GetData(3);
            cache.GetData(5);
            cache.GetData(2);

        }
    }
}
