using System.Collections.Generic;

namespace Data_Structures
{
    public class DoublyLinkedList<T>
    {
        public int Count { get; private set; }
        public Node First { get; set; }
        public Node Last { get; set; }

        public IEnumerable<Node> Forward()
        {
            Node current = First;
            while (current != null)
            {
                yield return current;
                current = current.Next;
            }
        }
        public IEnumerable<Node> Back()
        {
            Node current = Last;
            while (current != null)
            {
                yield return current;
                current = current.Previous;
            }
        }
        public void AddToBegin(T data)
        {
            if (First == null)
            {
                AddToEnd(data);
            }
            else
            {
                Node node = new Node(data);
                node.Next = First;
                First.Previous = node;
                First = node;
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
                node.Previous = Last;
            }
            Last = node;
            Count++;
        }
        public bool Remove(T data)
        {
            Node node = Search(data);

            if (node != null)
            {
                if (Count == 1)
                {
                    Clear();
                    return true;
                }
                if (node == First)
                {
                    First = node.Next;
                    First.Previous = null;
                }
                else if (node == Last)
                {
                    Last = node.Previous;
                    Last.Next = null;
                }
                else
                {
                    node.Previous.Next = node.Next;
                    node.Next.Previous = node.Previous;
                }
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
            foreach (Node node in Forward())
            {
                if (node.Data.Equals(data))
                {
                    return node;
                }
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
            public Node Previous { get; set; }
            public Node Next { get; set; }

            public override string ToString()
            {
                return Data.ToString();
            }
        }
    }
}