using System;
using System.Collections.Generic;

namespace Data_Structures
{
    public class ListQueue<T>
    {
        private List<T> list = new List<T>();

        public int Count { get => list.Count; }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }
        public void Add(T data)
        {
            list.Add(data);
        }
        public T Remove()
        {
            if (Count > 0)
            {
                T value = list[0];
                list.RemoveAt(0);
                return value;
            }
            throw new Exception("The queue is empty");
        }
        public void Clear()
        {
            list.Clear();
        }
        public T Peek()
        {
            if (Count > 0) return list[0];
            throw new Exception("The queue is empty");
        }
    }
}