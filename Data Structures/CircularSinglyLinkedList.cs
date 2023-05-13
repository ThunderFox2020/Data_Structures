﻿using System.Collections.Generic;

namespace Data_Structures
{
    public class CircularSinglyLinkedList<T>
    {
        public int Count { get; private set; }
        public Node First { get; set; }
        public Node Last { get; set; }

        public IEnumerator<Node> GetEnumerator()
        {
            Node current = First;
            do
            {
                yield return current;
                current = current.Next;
            } while (current != First);
        }
        public void AddToBegin(T data)
        {
            if (First == null)
            {
                AddToEnd(data);
            }
            else
            {
                Node node = new Node(data) { Next = First };
                First = node;
                Last.Next = First;
                Count++;
            }
        }
        public void AddToEnd(T data)
        {
            Node node = new Node(data);

            if (First == null)
            {
                First = node;
            }
            else
            {
                Last.Next = node;
            }
            Last = node;
            Last.Next = First;
            Count++;
        }
        public bool Remove(T data)
        {
            Node current = Search(data, out Node previous);

            if (current != null)
            {
                if (Count == 1)
                {
                    Clear();
                    return true;
                }
                if (current == First)
                {
                    First = current.Next;
                }
                else if (current == Last)
                {
                    Last = previous;
                }
                previous.Next = current.Next;
                Count--;
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Clear()
        {
            Count = 0;
            First = null;
            Last = null;
        }
        public Node Search(T data)
        {
            foreach (Node node in this)
            {
                if (node.Data.Equals(data))
                {
                    return node;
                }
            }
            return default;
        }
        
        private Node Search(T data, out Node previous)
        {
            previous = Last;
            foreach (Node node in this)
            {
                if (node.Data.Equals(data))
                {
                    return node;
                }
                previous = node;
            }
            return default;
        }

        public class Node
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