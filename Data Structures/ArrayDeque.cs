using System;
using System.Collections.Generic;
using System.Linq;

namespace Data_Structures
{
    public class ArrayDeque<T>
    {
        public ArrayDeque(int length = 10)
        {
            array = new T[length];
            minLength = length;
            first = length / 2;
            last = length / 2;
        }

        private T[] array;
        private int minLength;
        private int first;
        private int last;

        public int Count { get; private set; }

        public IEnumerable<T> BeginToEnd()
        {
            for (int i = first + 1; i <= last; i++)
            {
                yield return array[i];
            }
        }
        public IEnumerable<T> EndToBegin()
        {
            return BeginToEnd().Reverse();
        }
        public void AddToBegin(T data)
        {
            if (first <= 0)
            {
                T[] newArray = new T[array.Length + array.Length / 2];
                for (int i = first + 1, j = newArray.Length / 3 + 1; i <= last; i++, j++)
                {
                    newArray[j] = array[i];
                }
                array = newArray;
                first = newArray.Length / 3;
                last = first + Count;
            }
            array[first--] = data;
            Count++;
        }
        public void AddToEnd(T data)
        {
            if (last + 1 >= array.Length)
            {
                Array.Resize(ref array, array.Length + array.Length / 2);
            }
            array[++last] = data;
            Count++;
        }
        public T RemoveFromBegin()
        {
            if (minLength < Count && Count < first + 1)
            {
                T[] newArray = new T[array.Length - first + 1];
                for (int i = first + 1, j = 0; i <= last; i++, j++)
                {
                    newArray[j] = array[i];
                }
                array = newArray;
                first = -1;
                last = Count - 1;
            }
            if (Count == 0)
            {
                throw new Exception("The deque is empty");
            }
            T value = array[first + 1];
            if (Count == 1)
            {
                Clear();
            }
            else
            {
                array[first++ + 1] = default;
                Count--;
            }
            return value;
        }
        public T RemoveFromEnd()
        {
            if (minLength < Count && Count < array.Length - last - 1)
            {
                Array.Resize(ref array, last + 1);
            }
            if (Count == 0)
            {
                throw new Exception("The deque is empty");
            }
            T value = array[last];
            if (Count == 1)
            {
                Clear();
            }
            else
            {
                array[last--] = default;
                Count--;
            }
            return value;
        }
        public void Clear()
        {
            Array.Clear(array, 0, array.Length);
            Array.Resize(ref array, minLength);
            first = array.Length / 2;
            last = array.Length / 2;
            Count = 0;
        }
        public T PeekFromBegin()
        {
            if (Count > 0)
            {
                return array[first + 1];
            }
            throw new Exception("The deque is empty");
        }
        public T PeekFromEnd()
        {
            if (Count > 0)
            {
                return array[last];
            }
            throw new Exception("The deque is empty");
        }
    }
}