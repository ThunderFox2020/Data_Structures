using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Data_Structures
{
    public class TreeSet<T> : IEnumerable<T> where T : IComparable<T>
    {
        public TreeSet() { }
        
        private TreeSet(T data, TreeSet<T> root, TreeSet<T> parent, int depth)
        {
            Count++;
            this.data = data;
            this.root = root;
            this.parent = parent;
            this.depth = depth;
        }

        private T data;
        private TreeSet<T> root;
        private TreeSet<T> parent;
        private TreeSet<T> left;
        private TreeSet<T> right;
        private int depth;

        public int Count { get; private set; }

        public static TreeSet<T> Except(IEnumerable<T> collectionA, IEnumerable<T> collectionB)
        {
            TreeSet<T> setA = collectionA as TreeSet<T> ?? GetSet(collectionA);
            TreeSet<T> setB = collectionB as TreeSet<T> ?? GetSet(collectionB);

            TreeSet<T> result = new TreeSet<T>();
            foreach (T item in setA)
            {
                result.Add(item, false);
            }
            TreeSet<T> intersect = Intersect(setA, setB);
            foreach (T item in intersect)
            {
                result.Remove(item, false);
            }
            return result;
        }
        public static TreeSet<T> Intersect(IEnumerable<T> collectionA, IEnumerable<T> collectionB)
        {
            TreeSet<T> setA = collectionA as TreeSet<T> ?? GetSet(collectionA);
            TreeSet<T> setB = collectionB as TreeSet<T> ?? GetSet(collectionB);

            TreeSet<T> result = new TreeSet<T>();
            foreach (T item in setA)
            {
                if (setB.Contains(item))
                {
                    result.Add(item, false);
                }
            }
            return result;
        }
        public static TreeSet<T> SymmetricExcept(IEnumerable<T> collectionA, IEnumerable<T> collectionB)
        {
            TreeSet<T> setA = collectionA as TreeSet<T> ?? GetSet(collectionA);
            TreeSet<T> setB = collectionB as TreeSet<T> ?? GetSet(collectionB);

            TreeSet<T> result = Union(setA, setB);
            TreeSet<T> intersect = Intersect(setA, setB);
            foreach (T item in intersect)
            {
                result.Remove(item, false);
            }
            return result;
        }
        public static TreeSet<T> Union(IEnumerable<T> collectionA, IEnumerable<T> collectionB)
        {
            TreeSet<T> setA = collectionA as TreeSet<T> ?? GetSet(collectionA);
            TreeSet<T> setB = collectionB as TreeSet<T> ?? GetSet(collectionB);

            TreeSet<T> result = new TreeSet<T>();
            foreach (T item in setA)
            {
                result.Add(item, false);
            }
            foreach (T item in setB)
            {
                result.Add(item, false);
            }
            return result;
        }
        public static TreeSet<T> GetSet(IEnumerable<T> collection)
        {
            TreeSet<T> set = new TreeSet<T>();
            foreach (T item in collection)
            {
                set.Add(item, false);
            }
            return set;
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            foreach (TreeSet<T> tree in InfixTraversal())
            {
                yield return tree.data;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public bool Add(T data, bool withRebalancing = true)
        {
            bool result = false;
            Add(data, ref result);
            if (withRebalancing && RebalancingRequired()) Rebalancing();
            return result;
        }
        public bool Remove(T data, bool withRebalancing = true)
        {
            TreeSet<T> toRemove = null;
            Search(data, ref toRemove);

            if (toRemove != null)
            {
                List<T> toBack = new List<T>();
                foreach (TreeSet<T> item in toRemove.PrefixTraversal())
                {
                    toBack.Add(item.data);
                }
                toBack.RemoveAt(0);
                if (toRemove == root)
                {
                    Clear();
                }
                else
                {
                    if (toRemove == toRemove.parent.left)
                    {
                        toRemove.parent.left = null;
                    }
                    else if (toRemove == toRemove.parent.right)
                    {
                        toRemove.parent.right = null;
                    }
                    ReCount(toRemove.parent, -toRemove.Count);
                }
                foreach (T item in toBack)
                {
                    Add(item, false);
                }
                if (withRebalancing && RebalancingRequired()) Rebalancing();
                return true;
            }
            return false;
        }
        public void Clear()
        {
            Count = 0;
            data = default;
            root = null;
            parent = null;
            left = null;
            right = null;
            depth = 0;
        }
        public bool Getter(T data, out T result)
        {
            TreeSet<T> required = null;
            Search(data, ref required);
            if (required != null)
            {
                result = required.data;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }
        public bool Setter(T before, T after)
        {
            TreeSet<T> requiredAfter = null;
            Search(after, ref requiredAfter);
            if (requiredAfter == null)
            {
                if (Remove(before, false))
                {
                    Add(after, false);
                    return true;
                }
            }
            return false;
        }
        public void Rebalancing()
        {
            List<Balancer> balancers = new List<Balancer>();
            foreach (T item in this)
            {
                balancers.Add(new Balancer(false, item));
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
            if (left != null)
            {
                TreeSet<T> current = left;
                while (true)
                {
                    if (current.left != null)
                    {
                        current = current.left;
                        continue;
                    }
                    else if (current.right != null)
                    {
                        current = current.right;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                leftDepth = current.depth;
            }
            if (right != null)
            {
                TreeSet<T> current = right;
                while (true)
                {
                    if (current.right != null)
                    {
                        current = current.right;
                        continue;
                    }
                    else if (current.left != null)
                    {
                        current = current.left;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                rightDepth = current.depth;
            }
            return Math.Abs(leftDepth - rightDepth) > 10;
        }
        public void Except(IEnumerable<T> collection)
        {
            TreeSet<T> result = Except(this, collection);
            Count = result.Count;
            data = result.data;
            root = result.root;
            parent = result.parent;
            left = result.left;
            right = result.right;
            depth = result.depth;
        }
        public void Intersect(IEnumerable<T> collection)
        {
            TreeSet<T> result = Intersect(this, collection);
            Count = result.Count;
            data = result.data;
            root = result.root;
            parent = result.parent;
            left = result.left;
            right = result.right;
            depth = result.depth;
        }
        public void SymmetricExcept(IEnumerable<T> collection)
        {
            TreeSet<T> result = SymmetricExcept(this, collection);
            Count = result.Count;
            data = result.data;
            root = result.root;
            parent = result.parent;
            left = result.left;
            right = result.right;
            depth = result.depth;
        }
        public void Union(IEnumerable<T> collection)
        {
            TreeSet<T> result = Union(this, collection);
            Count = result.Count;
            data = result.data;
            root = result.root;
            parent = result.parent;
            left = result.left;
            right = result.right;
            depth = result.depth;
        }
        public bool Overlaps(IEnumerable<T> collection)
        {
            TreeSet<T> set = collection as TreeSet<T> ?? GetSet(collection);
            foreach (T item in set)
            {
                if (this.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsEqualSet(IEnumerable<T> collection)
        {
            TreeSet<T> set = collection as TreeSet<T> ?? GetSet(collection);
            foreach (T item in set)
            {
                if (!this.Contains(item))
                {
                    return false;
                }
            }
            return Count == set.Count;
        }
        public bool IsSuperSet(IEnumerable<T> collection)
        {
            TreeSet<T> subSet = collection as TreeSet<T> ?? GetSet(collection);
            foreach (T item in subSet)
            {
                if (!this.Contains(item))
                {
                    return false;
                }
            }
            return true;
        }
        public bool IsProperSuperSet(IEnumerable<T> collection)
        {
            TreeSet<T> subSet = collection as TreeSet<T> ?? GetSet(collection);
            foreach (T item in subSet)
            {
                if (!this.Contains(item))
                {
                    return false;
                }
            }
            return Count != subSet.Count;
        }
        public bool IsSubSet(IEnumerable<T> collection)
        {
            TreeSet<T> superSet = collection as TreeSet<T> ?? GetSet(collection);
            foreach (T item in this)
            {
                if (!superSet.Contains(item))
                {
                    return false;
                }
            }
            return true;
        }
        public bool IsProperSubSet(IEnumerable<T> collection)
        {
            TreeSet<T> superSet = collection as TreeSet<T> ?? GetSet(collection);
            foreach (T item in this)
            {
                if (!superSet.Contains(item))
                {
                    return false;
                }
            }
            return Count != superSet.Count;
        }
        
        private void Add(T data, ref bool result)
        {
            if (Count == 0)
            {
                Count++;
                this.data = data;
                root = this;
                depth++;
                result = true;
                return;
            }
            if (data.CompareTo(data) < 0)
            {
                if (left == null)
                {
                    left = new TreeSet<T>(data, root, this, depth + 1);
                    ReCount(this, 1);
                    result = true;
                }
                else
                {
                    left.Add(data, ref result);
                }
            }
            else if (data.CompareTo(data) > 0)
            {
                if (right == null)
                {
                    right = new TreeSet<T>(data, root, this, depth + 1);
                    ReCount(this, 1);
                    result = true;
                }
                else
                {
                    right.Add(data, ref result);
                }
            }
        }
        private void Search(T data, ref TreeSet<T> result)
        {
            if (Count != 0)
            {
                if (data.CompareTo(data) == 0)
                {
                    result = this;
                }
                else if (data.CompareTo(data) < 0)
                {
                    left?.Search(data, ref result);
                }
                else if (data.CompareTo(data) > 0)
                {
                    right?.Search(data, ref result);
                }
            }
        }
        private void ReCount(TreeSet<T> start, int count)
        {
            TreeSet<T> current = start;

            while (current != null)
            {
                current.Count += count;
                current = current.parent;
            }
        }
        private IEnumerable<TreeSet<T>> PrefixTraversal(List<TreeSet<T>> result = null)
        {
            if (result == null) result = new List<TreeSet<T>>();
            if (Count != 0) result.Add(this);
            left?.PrefixTraversal(result);
            right?.PrefixTraversal(result);
            return result;
        }
        private IEnumerable<TreeSet<T>> InfixTraversal(List<TreeSet<T>> result = null)
        {
            if (result == null) result = new List<TreeSet<T>>();
            left?.InfixTraversal(result);
            if (Count != 0) result.Add(this);
            right?.InfixTraversal(result);
            return result;
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