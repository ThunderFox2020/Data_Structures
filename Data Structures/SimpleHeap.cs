using System;
using System.Collections.Generic;

namespace Data_Structures
{
    public class SimpleHeap<T> where T : IComparable<T>
    {
        private T[] array = new T[10];

        public int Count { get; private set; }

        public void Add(T data)
        {
            if (Count + 1 >= array.Length)
                Array.Resize(ref array, array.Length * 2);

            array[Count++] = data;

            int child = Count - 1;
            BalancingBottomToTop(ref child);
        }
        public bool Remove(T data)
        {
            int required = Search(data);
            if (required != -1)
            {
                if (Count == 1)
                {
                    Clear();
                    return true;
                }
                else
                {
                    array[required] = array[Count - 1];
                    array[Count - 1] = default;
                    Count--;

                    BalancingTopToBottom(ref required);
                    BalancingBottomToTop(ref required);

                    if (10 < Count && Count < array.Length / 2)
                        Array.Resize(ref array, array.Length / 2);

                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        public void Clear()
        {
            array = new T[10];
            Count = 0;
        }
        public bool Search(T data, out T result)
        {
            int required = Search(data);
            if (required != -1)
            {
                result = array[required];
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }
        public IEnumerable<T> HorizontalTraversal()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return array[i];
            }
        }
        public IEnumerable<T> PrefixTraversal(int parent = 0, List<T> result = null)
        {
            int left = parent * 2 + 1;
            int right = parent * 2 + 2;

            if (result == null) result = new List<T>();
            result.Add(array[parent]);
            if (ChildExists(left)) PrefixTraversal(left, result);
            if (ChildExists(right)) PrefixTraversal(right, result);
            return result;
        }
        public IEnumerable<T> InfixTraversal(int parent = 0, List<T> result = null)
        {
            int left = parent * 2 + 1;
            int right = parent * 2 + 2;

            if (result == null) result = new List<T>();
            if (ChildExists(left)) InfixTraversal(left, result);
            result.Add(array[parent]);
            if (ChildExists(right)) InfixTraversal(right, result);
            return result;
        }
        public IEnumerable<T> PostfixTraversal(int parent = 0, List<T> result = null)
        {
            int left = parent * 2 + 1;
            int right = parent * 2 + 2;

            if (result == null) result = new List<T>();
            if (ChildExists(left)) PostfixTraversal(left, result);
            if (ChildExists(right)) PostfixTraversal(right, result);
            result.Add(array[parent]);
            return result;
        }
        
        private int Search(T data)
        {
            if (Count != 0)
            {
                if (data.CompareTo(array[Count / 2]) == 0)
                {
                    return Count / 2;
                }
                else if (data.CompareTo(array[Count / 2]) > 0)
                {
                    for (int i = 0; i <= Count - 1; i++)
                    {
                        if (data.Equals(array[i]))
                            return i;
                    }
                    return -1;
                }
                else
                {
                    for (int i = Count - 1; i >= 0; i--)
                    {
                        if (data.Equals(array[i]))
                            return i;
                    }
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }
        private void BalancingBottomToTop(ref int child)
        {
            int parent = (child - 1) / 2;
            while (0 <= parent && parent <= Count - 1)
            {
                if (array[child].CompareTo(array[parent]) > 0)
                    (array[child], array[parent]) = (array[parent], array[child]);
                else
                    break;
                child = parent;
                parent = (child - 1) / 2;
            }
        }
        private void BalancingTopToBottom(ref int parent)
        {
            int leftChild = parent * 2 + 1;
            int rightChild = parent * 2 + 2;
            int max;

            while (0 <= leftChild && leftChild <= Count - 1)
            {
                if (!(0 <= rightChild && rightChild <= Count - 1))
                    max = leftChild;
                else
                    max = array[leftChild].CompareTo(array[rightChild]) >= 0 ? leftChild : rightChild;

                if (array[parent].CompareTo(array[max]) < 0)
                {
                    (array[parent], array[max]) = (array[max], array[parent]);
                    parent = max;
                    leftChild = parent * 2 + 1;
                    rightChild = parent * 2 + 2;
                }
                else
                {
                    break;
                }
            }
        }
        private bool ChildExists(int child)
        {
            return 0 <= child && child <= Count - 1;
        }
    }
}