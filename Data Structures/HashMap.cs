using System;
using System.Collections.Generic;

namespace Data_Structures
{
    public class HashMap<TKey, TValue>
    {
        public HashMap() : this(10, 0.5) { }
        public HashMap(int minLength) : this(minLength, 0.5) { }
        public HashMap(double loadFactor) : this(10, loadFactor) { }
        public HashMap(int minLength, double loadFactor)
        {
            array = new Pair[minLength];
            this.minLength = minLength;
            this.loadFactor = loadFactor;
        }

        private Pair[] array;
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
            foreach (Pair pair in array)
            {
                if (pair != null)
                {
                    yield return pair;
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
            if (array[hash] == null)
            {
                array[hash] = new Pair(key, value);
                Count++;
            }
            else
            {
                if (!array[hash].Key.Equals(key))
                {
                    for (int i = hash + 1; i != hash; i++)
                    {
                        if (i >= array.Length)
                        {
                            i = 0;
                        }
                        if (array[i] == null)
                        {
                            array[i] = new Pair(key, value);
                            Count++;
                            return;
                        }
                        if (array[i].Key.Equals(key))
                        {
                            throw new Exception("The key already exists");
                        }
                    }
                }
                throw new Exception("The key already exists");
            }
        }
        public void Remove(object type)
        {
            if (type is not TKey and not TValue)
            {
                throw new Exception("Invalid type");
            }
            if (type is TKey key)
            {
                array[SearchPairByKey(key)] = null;
            }
            else if (type is TValue value)
            {
                array[SearchPairByValue(value)] = null;
            }
            Count--;
            if (minLength < Count && Count < array.Length / 2 * loadFactor)
            {
                Defragmentation(array.Length / 2);
            }
        }
        public void Clear()
        {
            array = new Pair[minLength];
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
                return array[SearchPairByKey(key)];
            }
            else if (type is TValue value)
            {
                return array[SearchPairByValue(value)];
            }
            else
            {
                throw new Exception("Invalid type");
            }
        }
        
        private int SearchPairByKey(TKey key)
        {
            int hash = GetHash(key, array.Length);

            if (array[hash] != null && array[hash].Key.Equals(key))
            {
                return hash;
            }
            else
            {
                for (int i = hash + 1; i != hash; i++)
                {
                    if (i >= array.Length)
                    {
                        i = 0;
                    }
                    if (array[i] != null && array[i].Key.Equals(key))
                    {
                        return i;
                    }
                }
                throw new Exception("The key does not exist");
            }
        }
        private int SearchPairByValue(TValue value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != null && array[i].Value.Equals(value))
                {
                    return i;
                }
            }
            throw new Exception("The value does not exist");
        }
        private void Defragmentation(int size)
        {
            Pair[] newArray = new Pair[size];
            foreach (Pair pair in this)
            {
                int hash = GetHash(pair.Key, newArray.Length);
                if (newArray[hash] == null)
                {
                    newArray[hash] = pair;
                }
                else
                {
                    for (int i = hash + 1; i != hash; i++)
                    {
                        if (i >= newArray.Length)
                        {
                            i = 0;
                        }
                        if (newArray[i] == null)
                        {
                            newArray[i] = pair;
                            break;
                        }
                    }
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