using System;
using System.Collections.Generic;

namespace Data_Structures
{
    public class HashTable<TKey, TValue>
    {
        public HashTable() : this(10, 0.5) { }
        public HashTable(int minLength) : this(minLength, 0.5) { }
        public HashTable(double loadFactor) : this(10, loadFactor) { }
        public HashTable(int minLength, double loadFactor)
        {
            array = new List<Pair>[minLength];
            this.minLength = minLength;
            this.loadFactor = loadFactor;
        }

        private List<Pair>[] array;
        private int minLength;
        private double loadFactor;

        public int Count { get; private set; }

        public TValue this[TKey key]
        {
            get => SearchPair(key).Value;
            set => SearchPair(key).Value = value;
        }

        public IEnumerator<Pair> GetEnumerator()
        {
            foreach (List<Pair> pairs in array)
            {
                if (pairs != null)
                {
                    foreach (Pair pair in pairs)
                    {
                        yield return pair;
                    }
                }
            }
        }
        public void Add(TKey key, TValue value)
        {
            if (Count >= array.Length * loadFactor)
            {
                Defragmentation(array.Length * 2);
            }
            int hash = GetHash(key, array.Length);
            Pair pair = new Pair(key, value);
            if (array[hash] == null)
            {
                array[hash] = new List<Pair>() { pair };
            }
            else
            {
                if (array[hash].Exists((pair) => { return pair.Key.Equals(key); }))
                {
                    throw new Exception("The key already exists");
                }
                else
                {
                    array[hash].Add(pair);
                }
            }
            Count++;
        }
        public void Remove(object type)
        {
            if (type is TKey key)
            {
                RemoveByKey(key);
            }
            else if (type is TValue value)
            {
                RemoveByValue(value);
            }
            else
            {
                throw new Exception("Invalid type");
            }
            if (minLength < Count && Count < array.Length / 2 * loadFactor)
            {
                Defragmentation(array.Length / 2);
            }
        }
        public void Clear()
        {
            array = new List<Pair>[minLength];
            Count = 0;
        }
        public TKey SearchKey(TValue value)
        {
            return SearchPair(value).Key;
        }
        public TValue SearchValue(TKey key)
        {
            return SearchPair(key).Value;
        }
        public Pair SearchPair(object type)
        {
            if (type is TKey key)
            {
                return SearchPairByKey(key);
            }
            else if (type is TValue value)
            {
                return SearchPairByValue(value);
            }
            else
            {
                throw new Exception("Invalid type");
            }
        }
        
        private void RemoveByKey(TKey key)
        {
            Pair pair = SearchPair(key);
            int hash = GetHash(key, array.Length);
            Remove(hash, pair);
        }
        private void RemoveByValue(TValue value)
        {
            Pair pair = SearchPair(value);
            int hash = GetHash(pair.Key, array.Length);
            Remove(hash, pair);
        }
        private void Remove(int hash, Pair pair)
        {
            array[hash].Remove(pair);
            Count--;

            if (array[hash].Count == 0)
            {
                array[hash] = default;
            }
        }
        private Pair SearchPairByKey(TKey key)
        {
            int hash = GetHash(key, array.Length);
            if (array[hash] != null)
            {
                Pair pair = default;
                foreach (Pair p in array[hash])
                {
                    if (p.Key.Equals(key))
                    {
                        pair = p;
                    }
                }
                if (pair != null)
                {
                    return pair;
                }
            }
            throw new Exception("The key does not exist");
        }
        private Pair SearchPairByValue(TValue value)
        {
            foreach (Pair pair in this)
            {
                if (pair.Value.Equals(value))
                {
                    return pair;
                }
            }
            throw new Exception("The value does not exist");
        }
        private void Defragmentation(int size)
        {
            List<Pair>[] newArray = new List<Pair>[size];
            foreach (Pair pair in this)
            {
                int hash = GetHash(pair.Key, newArray.Length);
                if (newArray[hash] == null)
                {
                    newArray[hash] = new List<Pair>() { pair };
                }
                else
                {
                    newArray[hash].Add(pair);
                }
            }
            array = newArray;
        }
        private int GetHash(TKey key, int length)
        {
            int hash = key.GetHashCode() % length;
            return hash >= 0 ? hash : -hash;
        }

        public class Pair
        {
            public Pair(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }

            public TKey Key { get; }
            public TValue Value { get; set; }

            public override string ToString()
            {
                return $"{Key} - {Value}";
            }
        }
    }
}