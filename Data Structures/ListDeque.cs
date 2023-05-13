using System;
using System.Collections.Generic;
using System.Linq;

namespace Data_Structures
{
    public class ListDeque<T>
    {
        private List<T> list = new List<T>();

        public int Count { get => list.Count; }

        public IEnumerable<T> BeginToEnd()
        {
            return list;
        }
        public IEnumerable<T> EndToBegin()
        {
            return (from item in list select item).Reverse();
        }
        public void AddToBegin(T data)
        {
            list.Insert(0, data);
        }
        public void AddToEnd(T data)
        {
            list.Insert(Count, data);
        }
        public T RemoveFromBegin()
        {
            if (Count > 0)
            {
                T value = list[0];
                list.RemoveAt(0);
                return value;
            }
            throw new Exception("The deque is empty");
        }
        public T RemoveFromEnd()
        {
            if (Count > 0)
            {
                T value = list[^1];
                list.RemoveAt(Count - 1);
                return value;
            }
            throw new Exception("The deque is empty");
        }
        public void Clear()
        {
            list.Clear();
        }
        public T PeekFromBegin()
        {
            if (Count > 0)
            {
                return list[0];
            }
            throw new Exception("The deque is empty");
        }
        public T PeekFromEnd()
        {
            if (Count > 0)
            {
                return list[^1];
            }
            throw new Exception("The deque is empty");
        }
    }
}