using System.Collections.Generic;
using LeastRecentCache;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LRUCacheUnitTests
{
    [TestClass()]
    public class LeastRecentCacheTests
    {
        private LeastRecentCache<int, int> _cache;
        private MockDataProvider<int, int> _provider;
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
            _cache = LeastRecentCache<int, int>.Instance;
            _cache.ResizeCache(size);
            var data = new Dictionary<int, int>() {{key1, val1}, {key2, val2}, {key3, val3}, {key4, val4}};
            _provider = new MockDataProvider<int, int>(data);
            _cache.RegisterDataProvider(_provider);
        }

        [TestMethod()]
        public void SingletonTest()
        {
            var cache1 = LeastRecentCache<int, int>.Instance;
            var cache2 = LeastRecentCache<float, int>.Instance;
            Assert.AreSame(cache1, _cache);
            Assert.AreNotSame(cache2, _cache);
            Assert.AreEqual(_cache.GetType(), typeof(LeastRecentCache<int, int>));
        }

        [TestMethod()]
        public void GetDataTest()
        {
            Assert.AreEqual(val1, _cache.GetData(key1)); //from provider
            Assert.AreEqual(val1, _cache.GetData(key1)); //from cache
        }

        [TestMethod]
        public void GetCacheFillTest()
            //Composite test 
        {
            Assert.AreEqual(0, _cache.GetCacheFill());

            _cache.GetData(key1);

            Assert.AreEqual(1, _cache.GetCacheFill());

            _cache.GetData(key2);
            _cache.GetData(key3);

            Assert.AreEqual(2, _cache.GetCacheFill());
        }

        [TestMethod()]
        public void ClearCacheTest()
        {
            _cache.GetData(key2);
            _cache.GetData(key3);

            _cache.ClearCache();
            Assert.AreEqual(0, _cache.GetCacheFill());
        }

        [TestMethod()]
        public void ResizeCacheTest()
        {
            _cache.GetData(key1);
            _cache.GetData(key2);
            _cache.GetData(key3);
            _cache.ResizeCache(1);
            Assert.AreEqual(1, _cache.GetCacheFill());
        }

        [TestMethod]
        public void ElementDroppingTest()
        {
            LeastRecentCache<int, int>.Instance.GetData(1);
            LeastRecentCache<int, int>.Instance.GetData(2);
            List<int> expected1 = new List<int> {1, 2};
            CollectionAssert.AreEqual(expected1, _cache.GetCacheKeys());

            LeastRecentCache<int, int>.Instance.GetData(2);
            CollectionAssert.AreEqual(expected1, _cache.GetCacheKeys());

            LeastRecentCache<int, int>.Instance.GetData(1);
            List<int> expected2 = new List<int> {2, 1};
            CollectionAssert.AreEqual(expected2, _cache.GetCacheKeys());

            LeastRecentCache<int, int>.Instance.GetData(3);
            List<int> expected3 = new List<int> {1, 3};
            CollectionAssert.AreEqual(expected3, _cache.GetCacheKeys());

            LeastRecentCache<int, int>.Instance.GetData(2);
            List<int> expected4 = new List<int> {3, 2};
            CollectionAssert.AreEqual(expected4, _cache.GetCacheKeys());
        }

        [TestMethod]
        public void NonExistingKeyTest()
        {

            int val = LeastRecentCache<int, int>.Instance.GetData(-1);
            int expected = default;

            Assert.AreEqual(expected, val);
        }


        [TestMethod]
        public void ZeroSizeTest()
        {
            _cache.ResizeCache(0);
            _cache.GetData(key1);
            Assert.AreEqual(0, _cache.GetCacheFill());
        }
    }
}