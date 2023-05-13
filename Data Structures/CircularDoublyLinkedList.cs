using System.Collections.Generic;

namespace Data_Structures
{
    public class CircularDoublyLinkedList<T>
    {
        public int Count { get; private set; }
        public Node First { get; set; }

        public IEnumerable<Node> Forward()
        {
            Node current = First;
            do
            {
                yield return current;
                current = current.Next;
            } while (current != First);
        }
        public IEnumerable<Node> Back()
        {
            Node current = First.Previous;
            do
            {
                yield return current;
                current = current.Previous;
            } while (current != First.Previous);
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
                node.Previous = First.Previous;
                node.Next = First;
                First.Previous.Next = node;
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
                node.Previous = node;
                node.Next = node;
                First = node;
            }
            else
            {
                node.Previous = First.Previous;
                node.Next = First;
                First.Previous.Next = node;
                First.Previous = node;
            }
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
                    First.Previous.Next = First.Next;
                    First.Next.Previous = First.Previous;
                    First = First.Next;
                }
                else if (node == First.Previous)
                {
                    First.Previous.Previous.Next = First;
                    First.Previous = First.Previous.Previous;
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