
using System.Collections.Generic;

namespace LeastRecentCache
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = new Dictionary<int, int>() { { 1, 100 }, { 2, 200 }, { 3, 300 }, { 4, 400 } };
            MockDataProvider<int, int> provider = new MockDataProvider<int, int>(data);

            int val;
            LeastRecentCache<int, int>.Instance.RegisterDataProvider(provider);
            LeastRecentCache<int, int>.Instance.ResizeCache(2);
            LeastRecentCache<int, int>.Instance.RegisterLogger(new MockLogger());
            val = LeastRecentCache<int, int>.Instance.GetData(1);
            val = LeastRecentCache<int, int>.Instance.GetData(2);
            val = LeastRecentCache<int, int>.Instance.GetData(2);
            val = LeastRecentCache<int, int>.Instance.GetData(1);
            val = LeastRecentCache<int, int>.Instance.GetData(3);
            val = LeastRecentCache<int, int>.Instance.GetData(5);
            val = LeastRecentCache<int, int>.Instance.GetData(2);

        }
    }
}
