using System.Collections.Generic;
using LeastRecentCache;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LRUCacheUnitTests
{
    [TestClass()]
    public class LeastRecentCacheTests
    {
        private LeastRecentCache<int, int> cache;
        private MockDataProvider<int, int> provider;
        int key1 = 1;
        int key2 = 2;
        int key3 = 3;
        int key4 = 4;

        int val1 = 100;
        int val2 = 200;
        int val3 = 300;
        int val4 = 400;

        private int size = 2;

        [TestInitialize()]
        public void LRCache()
        {
            cache = LeastRecentCache<int, int>.Instance;
            cache.ResizeCache(size);
            var data = new Dictionary<int, int>() {{key1, val1}, {key2, val2}, {key3, val3}, {key4, val4}};
            provider = new MockDataProvider<int, int>(data);
            cache.RegisterDataProvider(provider);
        }

        [TestMethod()]
        public void SingletonTest()
        {
            var cache1 = LeastRecentCache<int, int>.Instance;
            var cache2 = LeastRecentCache<float, int>.Instance;
            Assert.AreSame(cache1, cache);
            Assert.AreNotSame(cache2, cache);
            Assert.AreEqual(cache.GetType(), typeof(LeastRecentCache<int, int>));
        }

        [TestMethod()]
        public void GetDataTest()
        {
            Assert.AreEqual(val1, cache.GetData(key1)); //from provider
            Assert.AreEqual(val1, cache.GetData(key1)); //from cache
        }

        [TestMethod]
        public void GetCacheFillTest()
            //Composite test 
        {
            Assert.AreEqual(0, cache.GetCacheFill());

            cache.GetData(key1);

            Assert.AreEqual(1, cache.GetCacheFill());

            cache.GetData(key2);
            cache.GetData(key3);

            Assert.AreEqual(2, cache.GetCacheFill());
        }

        [TestMethod()]
        public void ClearCacheTest()
        {
            cache.GetData(key2);
            cache.GetData(key3);

            cache.ClearCache();
            Assert.AreEqual(0, cache.GetCacheFill());
        }

        [TestMethod()]
        public void ResizeCacheTest()
        {
            cache.GetData(key1);
            cache.GetData(key2);
            cache.GetData(key3);
            cache.ResizeCache(1);
            Assert.AreEqual(1, cache.GetCacheFill());
        }

        [TestMethod]
        public void ElementDroppingTest()
        {

            List<int> cacheElements = new List<int>();
            int val;

            val = LeastRecentCache<int, int>.Instance.GetData(1);
            val = LeastRecentCache<int, int>.Instance.GetData(2);
            List<int> expected1 = new List<int> {1, 2};
            CollectionAssert.AreEqual(expected1, cache.GetCacheKeys());

            val = LeastRecentCache<int, int>.Instance.GetData(2);
            CollectionAssert.AreEqual(expected1, cache.GetCacheKeys());

            val = LeastRecentCache<int, int>.Instance.GetData(1);
            List<int> expected2 = new List<int> {2, 1};
            CollectionAssert.AreEqual(expected2, cache.GetCacheKeys());

            val = LeastRecentCache<int, int>.Instance.GetData(3);
            List<int> expected3 = new List<int> {1, 3};
            CollectionAssert.AreEqual(expected3, cache.GetCacheKeys());

            val = LeastRecentCache<int, int>.Instance.GetData(2);
            List<int> expected4 = new List<int> {3, 2};
            CollectionAssert.AreEqual(expected4, cache.GetCacheKeys());
        }

        [TestMethod]
        public void NonExistingKeyTest()
        {

            int val = LeastRecentCache<int, int>.Instance.GetData(-1);
            int expected = default(int);

            Assert.AreEqual(expected, val);
        }


        [TestMethod]
        public void ZeroSizeTest()
        {
            cache.ResizeCache(0);
            cache.GetData(key1);
            Assert.AreEqual(0, cache.GetCacheFill());
        }
    }
}