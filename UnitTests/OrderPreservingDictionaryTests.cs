using System.Collections.Generic;
using LeastRecentCache;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LRUCacheUnitTests
{
    [TestClass()]
    public class OrderPreservingDictionaryTests
    {
        object key1 = new object();
        object key2 = new object();
        object key3 = new object();
        object key4 = new object();
        int val1 = 1;
        int val2 = 2;
        int val3 = 3;
        OrderPreservingDictionary<object,int> dict = new OrderPreservingDictionary<object, int>();

        [TestInitialize]
        public void OrderPreservingDictionaryTest()
        {
            dict.Add(key1, val1);
            dict.Add(key2, val2);
            dict.Add(key3, val3);
        }

        [TestMethod]
        public void AddTest()
        {
            //Adding items performed in intialiser as it is necessary set up for all subsequent tests

            Assert.AreEqual(1,dict[key1]);
            Assert.AreEqual(2, dict[key2]);
        }


        [TestMethod]
        public void ContainsKeyTest()
        {
            Assert.AreEqual(true, dict.ContainsKey(key1));
            Assert.AreEqual(false, dict.ContainsKey(key4));
        }

        [TestMethod]
        public void GetLastKeyTest()
        {
            Assert.AreEqual(key3,dict.GetLastKey());
        }

        [TestMethod]
        public void TryGetValueTest()
        {
            
            Assert.AreEqual(true, dict.TryGetValue(key1, out int testVal1));
            Assert.AreEqual(false, dict.TryGetValue(key4, out int testVal3));

            Assert.AreEqual(val1,testVal1);
            Assert.AreEqual(0, testVal3); //returns default int
        }

        [TestMethod]
        public void GetKeysTest()
        {
            var keyList = dict.GetKeys();

            CollectionAssert.AreEqual(new List<object>{key1,key2,key3},keyList);
        }



        [TestMethod]
        public void RemoveFirstTest()
        {
            dict.RemoveFirst();
            Assert.AreEqual(false, dict.ContainsKey(key1));
        }

        [TestMethod]
        public void RemoveTest()
        {
            dict.Remove(key2);

            var keyList = dict.GetKeys();

            CollectionAssert.AreEqual(new List<object> { key1, key3 }, keyList);
        }

        [TestMethod]
        public void ReinsertTest()
        {
            dict.Reinsert(key2);

            var keyList = dict.GetKeys();

            CollectionAssert.AreEqual(new List<object> { key1, key3,key2 }, keyList);
        }

        [TestMethod]
        public void RemoveMissingValue()
        {
            dict.Remove(new object());
            var keyList = dict.GetKeys();

            CollectionAssert.AreEqual(new List<object> { key1, key2, key3 }, keyList);
        }

        [TestMethod]
        public void RemoveFirstEmptyList()
        {
            OrderPreservingDictionary<object, int> dictEmpty = new OrderPreservingDictionary<object, int>();
            dictEmpty.RemoveFirst();
            var keyList = dictEmpty.GetKeys();

            CollectionAssert.AreEqual(new List<object> { }, keyList);
        }
    }
}

