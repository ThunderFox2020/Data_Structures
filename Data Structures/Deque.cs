using System;
using System.Collections.Generic;

namespace Data_Structures
{
    public class Deque<T>
    {
        private Node first;
        private Node last;

        public int Count { get; private set; }

        public IEnumerable<T> BeginToEnd()
        {
            Node current = first;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
        public IEnumerable<T> EndToBegin()
        {
            Node current = last;
            while (current != null)
            {
                yield return current.Data;
                current = current.Previous;
            }
        }
        public void AddToBegin(T data)
        {
            Node node = new Node(data);
            if (first == null)
            {
                first = node;
                last = node;
            }
            else
            {
                node.Next = first;
                first.Previous = node;
                first = node;
            }
            Count++;
        }
        public void AddToEnd(T data)
        {
            Node node = new Node(data);
            if (first == null)
            {
                AddToBegin(data);
            }
            else
            {
                node.Previous = last;
                last.Next = node;
                last = node;
                Count++;
            }
        }
        public T RemoveFromBegin()
        {
            if (Count < 1)
            {
                throw new Exception("The deque is empty");
            }
            T value = first.Data;
            if (Count == 1)
            {
                Clear();
            }
            else
            {
                first.Next.Previous = null;
                first = first.Next;
                Count--;
            }
            return value;
        }
        public T RemoveFromEnd()
        {
            if (Count < 1)
            {
                throw new Exception("The deque is empty");
            }
            T value = last.Data;
            if (Count == 1)
            {
                Clear();
            }
            else
            {
                last.Previous.Next = null;
                last = last.Previous;
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
        public T PeekFromBegin()
        {
            if (Count > 0) return first.Data;
            throw new Exception("The deque is empty");
        }
        public T PeekFromEnd()
        {
            if (Count > 0) return last.Data;
            throw new Exception("The deque is empty");
        }

        private class Node
        {
            public Node(T data)
            {
                Data = data;
            }

            public T Data { get; set; }
            public Node Previous { get; set; }
            public Node Next { get; set; }

            public override string ToString()
            {
                return Data.ToString();
            }
        }
    }
}