



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace Pioneer.WxSdk
{
    public class KeyValueCollection<TKey, TValue> : IDictionary<TKey, TValue>
    {
        Dictionary<TKey, TValue> innerDictionary;

        public KeyValueCollection()
        {
            innerDictionary = new Dictionary<TKey, TValue>();
        }

        public void Add(TKey key, TValue value)
        {

            innerDictionary.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return innerDictionary.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get { return innerDictionary.Keys; }
        }

        public bool Remove(TKey key)
        {
            return innerDictionary.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return innerDictionary.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values
        {
            get { return innerDictionary.Values; }
        }

        public TValue this[TKey key]
        {
            get
            {
                if (innerDictionary.ContainsKey(key))
                    return innerDictionary[key];
                else
                {
                    return default(TValue);
                }
            }
            set
            {
                innerDictionary[key] = value;
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            innerDictionary.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            innerDictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return innerDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if ((arrayIndex < 0) || (arrayIndex > array.Length))
            {
                throw new ArgumentOutOfRangeException();
            }
            if ((array.Length - arrayIndex) < this.Count)
            {
                throw new ArgumentException();
            }

            int currentIndex = 0;
            foreach (var item in innerDictionary)
            {
                KeyValuePair<TKey, TValue> pair = new KeyValuePair<TKey, TValue>();
                pair = item;
                array[arrayIndex + currentIndex] = pair;
                currentIndex++;
            }
        }

        public int Count
        {
            get { return innerDictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return innerDictionary.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return innerDictionary.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return innerDictionary.GetEnumerator();
        }
    }


    public class StringKeyValueCollection : KeyValueCollection<string, string>
    {

    }
}
