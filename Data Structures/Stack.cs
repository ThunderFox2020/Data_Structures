using System;
using System.Collections.Generic;

namespace Data_Structures
{
    public class Stack<T>
    {
        private Node upper;

        public int Count { get; private set; }

        public IEnumerator<T> GetEnumerator()
        {
            Node current = upper;
            while (current != null)
            {
                yield return current.Data;
                current = current.Down;
            }
        }
        public void Add(T data)
        {
            Node node = new Node(data);
            node.Down = upper;
            upper = node;
            Count++;
        }
        public T Remove()
        {
            if (Count > 0)
            {
                T value = upper.Data;
                upper = upper.Down;
                Count--;
                return value;
            }
            throw new Exception("The stack is empty");
        }
        public void Clear()
        {
            upper = null;
            Count = 0;
        }
        public T Peek()
        {
            if (Count > 0)
            {
                return upper.Data;
            }
            throw new Exception("The stack is empty");
        }

        private class Node
        {
            public Node(T data)
            {
                Data = data;
            }

            public T Data { get; set; }
            public Node Down { get; set; }

            public override string ToString()
            {
                return Data.ToString();
            }
        }
    }
}