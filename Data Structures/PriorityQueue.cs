using System;
using System.Collections.Generic;

namespace Data_Structures
{
    public class PriorityQueue<TPriority, TValue> where TPriority : IComparable<TPriority>
    {
        private Pair[] array = new Pair[10];

        public int Count { get; private set; }

        public IEnumerator<TValue> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
                yield return array[i].Value;
        }
        public void Add(TPriority priority, TValue value)
        {
            if (Count + 1 >= array.Length)
                Array.Resize(ref array, array.Length * 2);

            array[Count++] = new Pair(priority, value);

            int child = Count - 1;
            BalancingBottomToTop(ref child);
        }
        public TValue Remove()
        {
            if (Count == 0)
                throw new Exception("The queue is empty");
            TValue value = array[0].Value;
            if (Count == 1)
            {
                Clear();
            }
            else
            {
                array[0] = array[Count - 1];
                array[Count - 1] = default;
                Count--;

                int parent = 0;
                BalancingTopToBottom(ref parent);
                BalancingBottomToTop(ref parent);

                if (10 < Count && Count < array.Length / 2)
                    Array.Resize(ref array, array.Length / 2);
            }
            return value;
        }
        public void Clear()
        {
            array = new Pair[10];
            Count = 0;
        }
        public TValue Peek()
        {
            if (Count > 0)
                return array[0].Value;
            throw new Exception("The queue is empty");
        }
        
        private void BalancingBottomToTop(ref int child)
        {
            int parent = (child - 1) / 2;
            while (0 <= parent && parent <= Count - 1)
            {
                if (array[child].Priority.CompareTo(array[parent].Priority) > 0)
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
                    max = array[leftChild].Priority.CompareTo(array[rightChild].Priority) >= 0 ? leftChild : rightChild;

                if (array[parent].Priority.CompareTo(array[max].Priority) < 0)
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

        private class Pair
        {
            public Pair(TPriority priority, TValue value)
            {
                Priority = priority;
                Value = value;
            }

            public TPriority Priority { get; }
            public TValue Value { get; }
        }
    }
}