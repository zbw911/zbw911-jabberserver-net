using System.Collections;
using System.Collections.Generic;
using Jabber.Net.Server.Utils;

namespace Jabber.Net.Server.Collections
{
    class ReaderWriterLockDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> dic = new Dictionary<TKey, TValue>();
        private readonly ReaderWriterLock locker = new ReaderWriterLock();


        public ReaderWriterLockDictionary()
        {

        }

        public ReaderWriterLockDictionary(int capacity)
        {
            dic = new Dictionary<TKey, TValue>(capacity);
        }


        public TValue this[TKey key]
        {
            get
            {
                using (locker.ReadLock())
                {
                    return dic[key];
                }
            }
            set
            {
                using (locker.WriteLock())
                {
                    dic[key] = value;
                }
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                using (locker.ReadLock())
                {
                    return new List<TKey>(dic.Keys);
                }
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                using (locker.ReadLock())
                {
                    return new List<TValue>(dic.Values);
                }
            }
        }

        public int Count
        {
            get
            {
                using (locker.ReadLock())
                {
                    return dic.Count;
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                using (locker.ReadLock())
                {
                    return dic.IsReadOnly;
                }
            }
        }


        public bool ContainsKey(TKey key)
        {
            using (locker.ReadLock())
            {
                return dic.ContainsKey(key);
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            using (locker.ReadLock())
            {
                return dic.Contains(item);
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            using (locker.ReadLock())
            {
                return dic.TryGetValue(key, out value);
            }
        }

        public void Add(TKey key, TValue value)
        {
            using (locker.WriteLock())
            {
                dic.Add(key, value);
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            using (locker.WriteLock())
            {
                dic.Add(item);
            }
        }

        public bool Remove(TKey key)
        {
            using (locker.WriteLock())
            {
                return dic.Remove(key);
            }
        }

        public bool Remove(TKey key, out TValue value)
        {
            using (locker.WriteLock())
            {
                if (dic.TryGetValue(key, out value))
                {
                    return dic.Remove(key);
                }
                return false;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            using (locker.WriteLock())
            {
                return dic.Remove(item);
            }
        }

        public void Clear()
        {
            using (locker.WriteLock())
            {
                dic.Clear();
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            using (locker.WriteLock())
            {
                dic.CopyTo(array, arrayIndex);
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            using (locker.ReadLock())
            {
                return new Dictionary<TKey, TValue>(dic).GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
