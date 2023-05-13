using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Data_Structures
{
    public class HashSet<T> : IEnumerable<T>
    {
        public HashSet() : this(10, 0.5) { }
        public HashSet(int minLength) : this(minLength, 0.5) { }
        public HashSet(double loadFactor) : this(10, loadFactor) { }
        public HashSet(int minLength, double loadFactor)
        {
            array = new List<T>[minLength];
            this.minLength = minLength;
            this.loadFactor = loadFactor;
        }

        private List<T>[] array;
        private int minLength;
        private double loadFactor;

        public int Count { get; private set; }
        
        public static HashSet<T> Except(IEnumerable<T> collectionA, IEnumerable<T> collectionB)
        {
            HashSet<T> setA = collectionA as HashSet<T> ?? GetSet(collectionA);
            HashSet<T> setB = collectionB as HashSet<T> ?? GetSet(collectionB);

            HashSet<T> result = new HashSet<T>();
            foreach (T item in setA)
            {
                result.Add(item);
            }
            HashSet<T> intersect = Intersect(setA, setB);
            foreach (T item in intersect)
            {
                result.Remove(item);
            }
            return result;
        }
        public static HashSet<T> Intersect(IEnumerable<T> collectionA, IEnumerable<T> collectionB)
        {
            HashSet<T> setA = collectionA as HashSet<T> ?? GetSet(collectionA);
            HashSet<T> setB = collectionB as HashSet<T> ?? GetSet(collectionB);

            HashSet<T> result = new HashSet<T>();
            foreach (T item in setA)
            {
                if (setB.Contains(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }
        public static HashSet<T> SymmetricExcept(IEnumerable<T> collectionA, IEnumerable<T> collectionB)
        {
            HashSet<T> setA = collectionA as HashSet<T> ?? GetSet(collectionA);
            HashSet<T> setB = collectionB as HashSet<T> ?? GetSet(collectionB);

            HashSet<T> result = Union(setA, setB);
            HashSet<T> intersect = Intersect(setA, setB);
            foreach (T item in intersect)
            {
                result.Remove(item);
            }
            return result;
        }
        public static HashSet<T> Union(IEnumerable<T> collectionA, IEnumerable<T> collectionB)
        {
            HashSet<T> setA = collectionA as HashSet<T> ?? GetSet(collectionA);
            HashSet<T> setB = collectionB as HashSet<T> ?? GetSet(collectionB);

            HashSet<T> result = new HashSet<T>();
            foreach (T item in setA)
            {
                result.Add(item);
            }
            foreach (T item in setB)
            {
                result.Add(item);
            }
            return result;
        }
        public static HashSet<T> GetSet(IEnumerable<T> collection)
        {
            HashSet<T> set = new HashSet<T>();
            foreach (T item in collection)
            {
                set.Add(item);
            }
            return set;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (List<T> list in array)
            {
                if (list != null)
                {
                    foreach (T value in list)
                    {
                        yield return value;
                    }
                }
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public bool Add(T data)
        {
            if (Count >= array.Length * loadFactor)
            {
                Defragmentation(array.Length * 2);
            }

            int hash = GetHash(data, array.Length);
            if (array[hash] == null)
            {
                array[hash] = new List<T>() { data };
            }
            else
            {
                if (!array[hash].Contains(data))
                {
                    array[hash].Add(data);
                }
                else
                {
                    return false;
                }
            }
            Count++;
            return true;
        }
        public bool Remove(T data)
        {
            int hash = GetHash(data, array.Length);
            if (array[hash] != null)
            {
                bool result = array[hash].Remove(data);
                if (array[hash].Count == 0)
                {
                    array[hash] = default;
                }
                if (result)
                {
                    Count--;
                    if (minLength < Count && Count < array.Length / 2 * loadFactor)
                    {
                        Defragmentation(array.Length / 2);
                    }
                }
                return result;
            }
            return false;
        }
        public void Clear()
        {
            array = new List<T>[minLength];
            Count = 0;
        }
        public bool Getter(T data, out T result)
        {
            int hash = GetHash(data, array.Length);
            if (array[hash] != null)
            {
                int index = array[hash].IndexOf(data);
                if (index != -1)
                {
                    result = array[hash][index];
                    return true;
                }
            }
            result = default;
            return false;
        }
        public bool Setter(T before, T after)
        {
            int hash = GetHash(after, array.Length);
            if (!(array[hash] != null && array[hash].Contains(after)))
            {
                if (Remove(before))
                {
                    Add(after);
                    return true;
                }
            }
            return false;
        }
        public void Except(IEnumerable<T> collection)
        {
            HashSet<T> result = Except(this, collection);
            array = result.array;
            Count = result.Count;
        }
        public void Intersect(IEnumerable<T> collection)
        {
            HashSet<T> result = Intersect(this, collection);
            array = result.array;
            Count = result.Count;
        }
        public void SymmetricExcept(IEnumerable<T> collection)
        {
            HashSet<T> result = SymmetricExcept(this, collection);
            array = result.array;
            Count = result.Count;
        }
        public void Union(IEnumerable<T> collection)
        {
            HashSet<T> result = Union(this, collection);
            array = result.array;
            Count = result.Count;
        }
        public bool Overlaps(IEnumerable<T> collection)
        {
            HashSet<T> set = collection as HashSet<T> ?? GetSet(collection);
            foreach (T item in set)
            {
                if (this.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsEqualSet(IEnumerable<T> collection)
        {
            HashSet<T> set = collection as HashSet<T> ?? GetSet(collection);
            foreach (T item in set)
            {
                if (!this.Contains(item))
                {
                    return false;
                }
            }
            return Count == set.Count;
        }
        public bool IsSuperSet(IEnumerable<T> collection)
        {
            HashSet<T> subSet = collection as HashSet<T> ?? GetSet(collection);
            foreach (T item in subSet)
            {
                if (!this.Contains(item))
                {
                    return false;
                }
            }
            return true;
        }
        public bool IsProperSuperSet(IEnumerable<T> collection)
        {
            HashSet<T> subSet = collection as HashSet<T> ?? GetSet(collection);
            foreach (T item in subSet)
            {
                if (!this.Contains(item))
                {
                    return false;
                }
            }
            return Count != subSet.Count;
        }
        public bool IsSubSet(IEnumerable<T> collection)
        {
            HashSet<T> superSet = collection as HashSet<T> ?? GetSet(collection);
            foreach (T item in this)
            {
                if (!superSet.Contains(item))
                {
                    return false;
                }
            }
            return true;
        }
        public bool IsProperSubSet(IEnumerable<T> collection)
        {
            HashSet<T> superSet = collection as HashSet<T> ?? GetSet(collection);
            foreach (T item in this)
            {
                if (!superSet.Contains(item))
                {
                    return false;
                }
            }
            return Count != superSet.Count;
        }
        
        private void Defragmentation(int size)
        {
            List<T>[] newArray = new List<T>[size];
            foreach (T item in this)
            {
                int hash = GetHash(item, newArray.Length);
                if (newArray[hash] == null)
                {
                    newArray[hash] = new List<T>() { item };
                }
                else
                {
                    newArray[hash].Add(item);
                }
            }
            array = newArray;
        }
        private int GetHash(T key, int length)
        {
            int hash = key.GetHashCode() % length;
            return hash >= 0 ? hash : -hash;
        }
    }
}