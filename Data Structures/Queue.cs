using System;
using System.Collections.Generic;

namespace Data_Structures
{
    public class Queue<T>
    {
        private Node first;
        private Node last;

        public int Count { get; private set; }

        public IEnumerator<T> GetEnumerator()
        {
            Node current = first;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
        public void Add(T data)
        {
            Node node = new Node(data);
            if (first == null)
            {
                first = node;
            }
            else
            {
                last.Next = node;
            }
            last = node;
            Count++;
        }
        public T Remove()
        {
            if (Count < 1)
            {
                throw new Exception("The queue is empty");
            }
            T value = first.Data;
            if (Count == 1)
            {
                Clear();
            }
            else
            {
                first = first.Next;
                Count--;
            }
            return value;
        }
        public void Clear()
        {
            first = null;
            last = null;
            Count = 0;
        }
        public T Peek()
        {
            if (Count > 0) return first.Data;
            throw new Exception("The queue is empty");
        }

        private class Node
        {
            public Node(T data)
            {
                Data = data;
            }

            public T Data { get; set; }
            public Node Next { get; set; }

            public override string ToString()
            {
                return Data.ToString();
            }
        }
    }
}