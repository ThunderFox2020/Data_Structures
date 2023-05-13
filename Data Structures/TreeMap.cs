using System;
using System.Collections.Generic;

namespace Data_Structures
{
    public class TreeMap<TKey, TValue> where TKey : IComparable<TKey>
    {
        public TreeMap() { }
        
        private TreeMap(Pair data, TreeMap<TKey, TValue> root, TreeMap<TKey, TValue> parent, int depth)
        {
            Count++;
            this.data = data;
            this.root = root;
            this.parent = parent;
            this.depth = depth;
        }

        private Pair data;
        private TreeMap<TKey, TValue> root;
        private TreeMap<TKey, TValue> parent;
        private TreeMap<TKey, TValue> left;
        private TreeMap<TKey, TValue> right;
        private int depth;

        public int Count { get; private set; }

        public TValue this[TKey key]
        {
            get => SearchPair(key).Value;
            set => SearchPair(key).Value = value;
        }

        public IEnumerator<Pair> GetEnumerator()
        {
            foreach (TreeMap<TKey, TValue> tree in InfixTraversal())
            {
                yield return tree.data;
            }
        }
        public void Add(TKey key, TValue value, bool withRebalancing = true)
        {
            Add(key, value);
            if (withRebalancing && RebalancingRequired()) Rebalancing();
        }
        public void Remove(object type, bool withRebalancing = true)
        {
            if (type is not TKey and not TValue)
            {
                throw new Exception("Invalid type");
            }
            if (type is TKey key)
            {
                RemoveByKey(key);
            }
            else if (type is TValue value)
            {
                RemoveByValue(value);
            }
            if (withRebalancing && RebalancingRequired()) Rebalancing();
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
        public TKey SearchKey(TValue value)
        {
            TreeMap<TKey, TValue> tree = Search(value);
            if (tree != null) return tree.data.Key;
            else throw new Exception("The value does not exist");
        }
        public TValue SearchValue(TKey key)
        {
            TreeMap<TKey, TValue> tree = Search(key);
            if (tree != null) return tree.data.Value;
            else throw new Exception("The key does not exist");
        }
        public Pair SearchPair(object type)
        {
            TreeMap<TKey, TValue> tree = Search(type);
            if (tree != null) return tree.data;
            else throw new Exception("Pair not found");
        }
        public void Rebalancing()
        {
            List<Balancer> balancers = new List<Balancer>();
            foreach (Pair item in this)
            {
                balancers.Add(new Balancer(false, item));
            }
            Clear();
            Balancer startBalancer = balancers[balancers.Count / 2];
            Add(startBalancer.Data.Key, startBalancer.Data.Value, false);
            startBalancer.IsAdded = true;
            int step = balancers.Count / 4;
            while (true)
            {
                for (int i = step; i < balancers.Count; i += step)
                {
                    if (!balancers[i].IsAdded)
                    {
                        Add(balancers[i].Data.Key, balancers[i].Data.Value, false);
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
                TreeMap<TKey, TValue> current = left;
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
                TreeMap<TKey, TValue> current = right;
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
        
        private void Add(TKey key, TValue value)
        {
            if (Count == 0)
            {
                Count++;
                data = new Pair(key, value);
                root = this;
                depth++;
                return;
            }
            if (key.CompareTo(data.Key) < 0)
            {
                if (left == null)
                {
                    left = new TreeMap<TKey, TValue>(new Pair(key, value), root, this, depth + 1);
                    ReCount(this, 1);
                }
                else
                {
                    left.Add(key, value);
                }
            }
            else if (key.CompareTo(data.Key) > 0)
            {
                if (right == null)
                {
                    right = new TreeMap<TKey, TValue>(new Pair(key, value), root, this, depth + 1);
                    ReCount(this, 1);
                }
                else
                {
                    right.Add(key, value);
                }
            }
            else
            {
                throw new Exception("The key already exists");
            }
        }
        private void Remove(TreeMap<TKey, TValue> toRemove)
        {
            List<Pair> toBack = new List<Pair>();
            foreach (TreeMap<TKey, TValue> item in toRemove.PrefixTraversal())
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
            foreach (Pair item in toBack)
            {
                Add(item.Key, item.Value, false);
            }
        }
        private void RemoveByKey(TKey key)
        {
            TreeMap<TKey, TValue> toRemove = Search(key);
            if (toRemove != null)
            {
                Remove(toRemove);
            }
            else
            {
                throw new Exception("The key does not exist");
            }
        }
        private void RemoveByValue(TValue value)
        {
            TreeMap<TKey, TValue> toRemove = Search(value);
            if (toRemove != null)
            {
                Remove(toRemove);
            }
            else
            {
                throw new Exception("The value does not exist");
            }
        }
        private TreeMap<TKey, TValue> Search(object type)
        {
            TreeMap<TKey, TValue> tree = null;
            if (type is TKey key)
            {
                SearchByKey(key, ref tree);
            }
            else if (type is TValue value)
            {
                SearchByValue(value, ref tree);
            }
            else
            {
                throw new Exception("Invalid type");
            }
            return tree;
        }
        private void SearchByKey(TKey key, ref TreeMap<TKey, TValue> result)
        {
            if (Count != 0)
            {
                if (key.CompareTo(data.Key) == 0)
                {
                    result = this;
                }
                else if (key.CompareTo(data.Key) < 0)
                {
                    left?.SearchByKey(key, ref result);
                }
                else if (key.CompareTo(data.Key) > 0)
                {
                    right?.SearchByKey(key, ref result);
                }
            }
        }
        private void SearchByValue(TValue value, ref TreeMap<TKey, TValue> result)
        {
            foreach (TreeMap<TKey, TValue> tree in PrefixTraversal())
            {
                if (value.Equals(tree.data.Value))
                {
                    result = tree;
                    break;
                }
            }
        }
        private void ReCount(TreeMap<TKey, TValue> start, int count)
        {
            TreeMap<TKey, TValue> current = start;

            while (current != null)
            {
                current.Count += count;
                current = current.parent;
            }
        }
        private IEnumerable<TreeMap<TKey, TValue>> PrefixTraversal(List<TreeMap<TKey, TValue>> result = null)
        {
            if (result == null) result = new List<TreeMap<TKey, TValue>>();
            if (Count != 0) result.Add(this);
            left?.PrefixTraversal(result);
            right?.PrefixTraversal(result);
            return result;
        }
        private IEnumerable<TreeMap<TKey, TValue>> InfixTraversal(List<TreeMap<TKey, TValue>> result = null)
        {
            if (result == null) result = new List<TreeMap<TKey, TValue>>();
            left?.InfixTraversal(result);
            if (Count != 0) result.Add(this);
            right?.InfixTraversal(result);
            return result;
        }

        public class Pair
        {
            public Pair(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }

            public TKey Key { get; }
            public TValue Value { get; set; }

            public override string ToString()
            {
                return $"{Key} - {Value}";
            }
        }
        
        private class Balancer
        {
            public Balancer(bool isAdded, Pair data)
            {
                IsAdded = isAdded;
                Data = data;
            }

            public bool IsAdded { get; set; }
            public Pair Data { get; set; }
        }
    }
}