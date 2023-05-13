using System;
using System.Collections.Generic;
using System.Text;

namespace Data_Structures
{
    public class Trie<T>
    {
        public Trie()
        {
            root = this;
        }
        
        private Trie(Trie<T> root, char symbol, bool isWord, T data)
        {
            this.root = root;
            this.symbol = symbol;
            this.isWord = isWord;
            this.data = data;
        }

        private Trie<T> root;
        private char symbol;
        private bool isWord;
        private T data;
        private Dictionary<char, Trie<T>> children = new Dictionary<char, Trie<T>>();

        public int Count { get; private set; }

        public T this[string key]
        {
            get => Search(key);
            set => Change(key).data = value;
        }

        public IEnumerator<Pair> GetEnumerator()
        {
            return Traversal().GetEnumerator();
        }
        public void Add(string key, T data)
        {
            Trie<T> result = default;
            bool success = default;
            Search(key, ref result, ref success);

            if (!success)
                Insert(key, data);
            else
                throw new Exception("The key already exists");
        }
        public void PartialRemove(string key)
        {
            bool success = false;
            PartialRemove(key, ref success);

            if (success)
                root.Count--;
            else
                throw new Exception("The key does not exist");
        }
        public void CompleteRemove(string key)
        {
            bool success = false;
            bool continueRemoving = true;
            CompleteRemove(key, ref success, ref continueRemoving);

            if (success)
                root.Count--;
            else
                throw new Exception("The key does not exist");
        }
        public void Clear()
        {
            children.Clear();
            Count = 0;
        }
        public T Search(string key)
        {
            Trie<T> result = default;
            bool success = default;
            Search(key, ref result, ref success);

            if (success)
                return result.data;
            else
                throw new Exception("The key does not exist");
        }
        
        private List<Pair> Traversal(StringBuilder stringBuilder = null, List<Pair> result = null)
        {
            if (stringBuilder == null)
                stringBuilder = new StringBuilder();
            if (result == null)
                result = new List<Pair>();

            foreach (var child in children)
            {
                stringBuilder.Append(child.Value.symbol);
                if (child.Value.isWord)
                    result.Add(new Pair(stringBuilder.ToString(), child.Value.data));
                child.Value.Traversal(stringBuilder, result);
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
            }

            return result;
        }
        private void Insert(string key, T data)
        {
            if (key.Length != 0)
            {
                char symbol = key[0];
                key = key.Remove(0, 1);
                bool isWord = key.Length == 0;

                if (children.ContainsKey(symbol))
                {
                    if (isWord)
                    {
                        children[symbol].isWord = true;
                        children[symbol].data = data;
                        root.Count++;
                    }
                    children[symbol].Insert(key, data);
                }
                else
                {
                    if (isWord)
                        root.Count++;

                    Trie<T> trie = new Trie<T>(root, symbol, isWord, isWord ? data : default);
                    children.Add(symbol, trie);
                    trie.Insert(key, data);
                }
            }
        }
        private void PartialRemove(string key, ref bool success)
        {
            if (key.Length != 0)
            {
                char symbol = key[0];
                key = key.Remove(0, 1);

                if (children.ContainsKey(symbol))
                {
                    if (key.Length == 0 && children[symbol].isWord)
                    {
                        children[symbol].isWord = false;
                        children[symbol].data = default;
                        success = true;
                    }
                    children[symbol].PartialRemove(key, ref success);
                }
            }
        }
        private void CompleteRemove(string key, ref bool success, ref bool continueRemoving)
        {
            if (key.Length != 0)
            {
                char symbol = key[0];
                key = key.Remove(0, 1);

                if (children.ContainsKey(symbol))
                {
                    if (key.Length == 0 && children[symbol].isWord)
                    {
                        if (children[symbol].children.Count == 0)
                        {
                            children.Remove(symbol);
                            success = true;
                        }
                        else
                        {
                            children[symbol].isWord = false;
                            children[symbol].data = default;
                            success = true;
                            continueRemoving = false;
                        }
                    }
                    if (children.ContainsKey(symbol))
                    {
                        children[symbol].CompleteRemove(key, ref success, ref continueRemoving);
                        if (continueRemoving && !children[symbol].isWord && children[symbol].children.Count == 0)
                            children.Remove(symbol);
                        else
                            continueRemoving = false;
                    }
                }
            }
        }
        private void Search(string key, ref Trie<T> trie, ref bool success)
        {
            if (key.Length != 0)
            {
                char symbol = key[0];
                key = key.Remove(0, 1);

                if (children.ContainsKey(symbol))
                {
                    if (key.Length == 0 && children[symbol].isWord)
                    {
                        trie = children[symbol];
                        success = true;
                    }
                    children[symbol].Search(key, ref trie, ref success);
                }
            }
        }
        private Trie<T> Change(string key)
        {
            Trie<T> result = default;
            bool success = default;
            Search(key, ref result, ref success);

            if (success)
                return result;
            else
                throw new Exception("The key does not exist");
        }

        public class Pair
        {
            public Pair(string key, T value)
            {
                Key = key;
                Value = value;
            }

            public string Key { get; }
            public T Value { get; }
        }
    }
}