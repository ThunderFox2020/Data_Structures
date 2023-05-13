using System;
using System.Collections.Generic;

namespace Data_Structures
{
    public class Tree<T> where T : IComparable<T>
    {
        public Tree() { }
        
        private Tree(T data, Tree<T> root, Tree<T> parent, int depth)
        {
            Data = data;
            Root = root;
            Parent = parent;
            Depth = depth;
            Count++;
        }

        public T Data { get; private set; }
        public Tree<T> Root { get; private set; }
        public Tree<T> Parent { get; private set; }
        public Tree<T> Left { get; private set; }
        public Tree<T> Right { get; private set; }
        public int Depth { get; private set; }
        public int Count { get; private set; }

        public void Add(T data, bool withRebalancing = true)
        {
            if (Count == 0)
            {
                Data = data;
                Root = this;
                Depth++;
                Count++;
            }
            else
            {
                Root.Insert(data);
            }
            if (withRebalancing && RebalancingRequired())
            {
                Rebalancing();
            }
        }
        public bool Remove(T data, bool withRebalancing = true)
        {
            Tree<T> root = Root;
            Tree<T> toRemove = Search(data);

            if (toRemove != null)
            {
                if (toRemove != Root)
                {
                    if (toRemove == toRemove.Parent.Left)
                    {
                        toRemove.Parent.Left = null;
                    }
                    else if (toRemove == toRemove.Parent.Right)
                    {
                        toRemove.Parent.Right = null;
                    }
                    ReCount(toRemove.Parent, -toRemove.Count);
                }

                List<T> toBack = new List<T>();

                foreach (Tree<T> item in toRemove.PrefixTraversal())
                {
                    toBack.Add(item.Data);
                    item.Data = default;
                    item.Root = null;
                    item.Parent = null;
                    item.Left = null;
                    item.Right = null;
                    item.Depth = 0;
                    item.Count = 0;
                }

                toBack.RemoveAt(0);

                if (Count != 0)
                {
                    foreach (T item in toBack) Add(item, false);
                }
                else
                {
                    foreach (T item in toBack) root.Add(item, false);
                }

                if (withRebalancing && RebalancingRequired())
                {
                    Rebalancing();
                }

                return true;
            }
            return false;
        }
        public void Clear()
        {
            if (this != Root)
            {
                if (this == Parent.Left)
                {
                    Parent.Left = null;
                }
                else if (this == Parent.Right)
                {
                    Parent.Right = null;
                }

                ReCount(Parent, -Count);
            }
            foreach (Tree<T> tree in PostfixTraversal())
            {
                tree.Data = default;
                tree.Root = null;
                tree.Parent = null;
                tree.Left = null;
                tree.Right = null;
                tree.Depth = 0;
                tree.Count = 0;
            }
        }
        public Tree<T> Search(T data)
        {
            Tree<T> result = null;
            Search(data, ref result);
            return result;
        }
        public void Rebalancing()
        {
            List<Balancer> balancers = new List<Balancer>();
            foreach (Tree<T> item in InfixTraversal())
            {
                balancers.Add(new Balancer(false, item.Data));
            }
            Clear();
            Add(balancers[balancers.Count / 2].Data, false);
            balancers[balancers.Count / 2].IsAdded = true;
            int step = balancers.Count / 4;
            while (true)
            {
                for (int i = step; i < balancers.Count; i += step)
                {
                    if (!balancers[i].IsAdded)
                    {
                        Add(balancers[i].Data, false);
                        balancers[i].IsAdded = true;
                    }
                    if (step == 0) i++;
                }
                if (step == 0) break;
                else step /= 2;
            }
        }
        public bool RebalancingRequired()
        {
            int leftDepth = default;
            int rightDepth = default;
            if (Left != null)
            {
                Tree<T> current = Left;
                while (true)
                {
                    if (current.Left != null)
                    {
                        current = current.Left;
                        continue;
                    }
                    else if (current.Right != null)
                    {
                        current = current.Right;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                leftDepth = current.Depth;
            }
            if (Right != null)
            {
                Tree<T> current = Right;
                while (true)
                {
                    if (current.Right != null)
                    {
                        current = current.Right;
                        continue;
                    }
                    else if (current.Left != null)
                    {
                        current = current.Left;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                rightDepth = current.Depth;
            }
            return Math.Abs(leftDepth - rightDepth) > 10;
        }
        public IEnumerable<Tree<T>> HorizontalTraversal()
        {
            Queue<Tree<T>> queue = new Queue<Tree<T>>();
            Tree<T> current = this;
            queue.Add(current);
            do
            {
                yield return queue.Remove();
                if (current.Left != null) queue.Add(current.Left);
                if (current.Right != null) queue.Add(current.Right);
                if (queue.Count > 0) current = queue.Peek();
            } while (queue.Count > 0);
        }
        public IEnumerable<Tree<T>> PrefixTraversal(List<Tree<T>> result = null)
        {
            if (result == null) result = new List<Tree<T>>();
            if (Count != 0) result.Add(this);
            Left?.PrefixTraversal(result);
            Right?.PrefixTraversal(result);
            return result;
        }
        public IEnumerable<Tree<T>> InfixTraversal(List<Tree<T>> result = null)
        {
            if (result == null) result = new List<Tree<T>>();
            Left?.InfixTraversal(result);
            if (Count != 0) result.Add(this);
            Right?.InfixTraversal(result);
            return result;
        }
        public IEnumerable<Tree<T>> PostfixTraversal(List<Tree<T>> result = null)
        {
            if (result == null) result = new List<Tree<T>>();
            Left?.PostfixTraversal(result);
            Right?.PostfixTraversal(result);
            if (Count != 0) result.Add(this);
            return result;
        }
        
        private void Insert(T data)
        {
            if (data.CompareTo(Data) < 0)
            {
                if (Left == null)
                {
                    Left = new Tree<T>(data, Root, this, Depth + 1);
                    ReCount(this, 1);
                }
                else
                {
                    Left.Insert(data);
                }
            }
            else
            {
                if (Right == null)
                {
                    Right = new Tree<T>(data, Root, this, Depth + 1);
                    ReCount(this, 1);
                }
                else
                {
                    Right.Insert(data);
                }
            }
        }
        private void Search(T data, ref Tree<T> result)
        {
            if (Count != 0)
            {
                if (data.CompareTo(Data) == 0)
                {
                    result = this;
                }
                else if (data.CompareTo(Data) < 0)
                {
                    Left?.Search(data, ref result);
                }
                else if (data.CompareTo(Data) > 0)
                {
                    Right?.Search(data, ref result);
                }
            }
        }
        private void ReCount(Tree<T> start, int count)
        {
            Tree<T> current = start;

            while (current != null)
            {
                current.Count += count;
                current = current.Parent;
            }
        }

        private class Balancer
        {
            public Balancer(bool isAdded, T data)
            {
                IsAdded = isAdded;
                Data = data;
            }

            public bool IsAdded { get; set; }
            public T Data { get; set; }
        }
    }
}