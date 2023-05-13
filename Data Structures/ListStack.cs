using System;
using System.Collections.Generic;
using System.Linq;

namespace Data_Structures
{
    public class ListStack<T>
    {
        private List<T> list = new List<T>();

        public int Count { get => list.Count; }

        public IEnumerator<T> GetEnumerator()
        {
            return (from item in list select item).Reverse().GetEnumerator();
        }
        public void Add(T data)
        {
            list.Add(data);
        }
        public T Remove()
        {
            if (Count > 0)
            {
                T value = list[Count - 1];
                list.RemoveAt(Count - 1);
                return value;
            }
            throw new Exception("The stack is empty");
        }
        public void Clear()
        {
            list.Clear();
        }
        public T Peek()
        {
            if (Count > 0)
            {
                return list[Count - 1];
            }
            throw new Exception("The stack is empty");
        }
    }
}