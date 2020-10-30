using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LeastRecentCache
{

    public class OrderPreservingDictionary<TM, T>
    {
        class DictEntry<K, V>
        {
            public readonly V Value;
            public DictEntry<K, V> PreviousEntry;
            public DictEntry<K, V> NextEntry;
            public readonly K Key;
            public DictEntry(K key, V val, DictEntry<K, V> previous, DictEntry<K, V> next)
            {
                Key = key;
                Value = val;
                PreviousEntry = previous;
                NextEntry = next;
                //Ensures consistency of linked list by changing the links upon creation:
                if (PreviousEntry != null) PreviousEntry.NextEntry = this;
                if (NextEntry != null) NextEntry.PreviousEntry = this;
            }
        }

        readonly Dictionary<TM, DictEntry<TM, T>> dict = new Dictionary<TM, DictEntry<TM, T>>();
        private readonly DictEntry<TM, T> _head;
        private readonly DictEntry<TM, T> _tail;

        public OrderPreservingDictionary()
        {
            //Synthetic tail and head of the linked ease to reduce the number of boundary conditions checks
            _head = new DictEntry<TM, T>(default, default, null, null);
            _tail = new DictEntry<TM, T>(default, default, _head, null);
            _head.NextEntry = _tail;
        }

        public T this[TM key] => dict[key].Value;


        public bool ContainsKey(TM request)
        {
            return dict.ContainsKey(request);
        }
        public TM GetLastKey()
        {
            return _tail.PreviousEntry.Key;
        }
        public int Count => dict.Count;

        public bool TryGetValue(TM key, out T value)
        {
            if (dict.TryGetValue(key, out DictEntry<TM, T> entry))
            {
                value = entry.Value;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        public List<TM> GetKeys()
        {
            var list = new List<TM>();
            DictEntry<TM, T> entry = _head;
            entry = entry.NextEntry;
            while (entry != _tail)
            {
                list.Add(entry.Key);
                entry = entry.NextEntry;
            }

            return list;
        }
        public void Add(TM key, T value)
        {
            var entry = new DictEntry<TM, T>(key, value, _tail.PreviousEntry, _tail);
            dict.Add(key, entry);
        }

        public void RemoveFirst()
        {
            DictEntry<TM, T> entry = _head.NextEntry;
            _head.NextEntry = entry.NextEntry;
            dict.Remove(entry.Key);

        }
        public void Remove(TM key)
        {
            DictEntry<TM, T> entry = dict[key];

            entry.PreviousEntry.NextEntry = entry.NextEntry;
            entry.NextEntry.PreviousEntry = entry.PreviousEntry;

            dict.Remove(key);

        }
        public void Reinsert(TM key)
        {

            DictEntry<TM, T> entry = dict[key];

            entry.PreviousEntry.NextEntry = entry.NextEntry;
            entry.NextEntry.PreviousEntry = entry.PreviousEntry;

            entry.NextEntry = _tail;
            entry.PreviousEntry = _tail.PreviousEntry;

            _tail.PreviousEntry.NextEntry = entry;
            _tail.PreviousEntry = entry;


        }
    }


}