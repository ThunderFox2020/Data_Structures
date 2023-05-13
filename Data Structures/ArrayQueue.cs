using System;
using System.Collections.Generic;

namespace Data_Structures
{
    public class ArrayQueue<T>
    {
        public ArrayQueue(int length = 10)
        {
            array = new T[length];
            minLength = length;
            first = 0;
            last = 0;
        }

        private T[] array;
        private int minLength;
        private int first;
        private int last;

        public int Count { get; private set; }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = first; i < last; i++)
            {
                yield return array[i];
            }
        }
        public void Add(T data)
        {
            if (last >= array.Length)
            {
                Array.Resize(ref array, array.Length + array.Length / 2);
            }
            array[last++] = data;
            Count++;
        }
        public T Remove()
        {
            if (first >= Count)
            {
                for (int i = first, j = 0; i <= last; i++, j++)
                {
                    array[j] = array[i];
                }
                first = 0;
                last = Count - 1;
                if (minLength < Count && Count < array.Length / 2)
                {
                    Array.Resize(ref array, array.Length - array.Length / 2);
                }
            }
            if (Count > 0)
            {
                T value = array[first];
                array[first++] = default;
                Count--;
                return value;
            }
            throw new Exception("The queue is empty");
        }
        public void Clear()
        {
            Array.Clear(array, 0, array.Length);
            Array.Resize(ref array, minLength);
            first = 0;
            last = 0;
            Count = 0;
        }
        public T Peek()
        {
            if (Count > 0)
            {
                return array[first];
            }
            throw new Exception("The stack is empty");
        }
    }
}