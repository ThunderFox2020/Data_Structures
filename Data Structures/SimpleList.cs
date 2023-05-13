using System;
using System.Collections.Generic;

namespace Data_Structures
{
    public class SimpleList<T>
    {
        private T[] array = new T[10];

        public int Capacity { get; private set; } = 10;
        public int Count { get; private set; } = 0;

        public T this[int index]
        {
            get
            {
                if (-1 < index && index < Count)
                {
                    return array[index];
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
            set
            {
                if (-1 < index && index < Count)
                {
                    array[index] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return array[i];
            }
        }
        public void Add(T data)
        {
            if (Count >= Capacity)
            {
                Array.Resize(ref array, Capacity + Capacity / 2);
                Capacity = array.Length;
            }
            array[Count++] = data;
        }
        public void Remove(int index)
        {
            if (-1 < index && index < Count)
            {
                int end = Count < Capacity ? Count : Count - 1;
                for (int i = index; i < end; i++)
                {
                    array[i] = array[i + 1];
                }
                if (Count == Capacity)
                {
                    array[Count - 1] = default;
                }
                Count--;
                if (10 < Count && Count < Capacity / 2)
                {
                    Array.Resize(ref array, Capacity - Capacity / 2);
                    Capacity = array.Length;
                }
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }
        public void Clear()
        {
            Array.Clear(array, 0, array.Length);
            Array.Resize(ref array, 10);
            Capacity = array.Length;
            Count = 0;
        }
    }
}