using System;
using System.Collections.Generic;

namespace Data_Structures
{
    public class ComplexHeap<T> where T : IComparable<T>
    {
        public ComplexHeap()
        {
            array = new ComplexHeap<T>[10];
        }

        private ComplexHeap(int i, T data, ComplexHeap<T> root, ComplexHeap<T> parent)
        {
            index = i;
            Data = data;
            Root = root;
            Parent = parent;
            Count++;
            Root.array[Root.Count] = this;
        }

        private ComplexHeap<T>[] array;
        private int index;

        public T Data { get; private set; }
        public ComplexHeap<T> Root { get; private set; }
        public ComplexHeap<T> Parent { get; private set; }
        public ComplexHeap<T> Left { get; private set; }
        public ComplexHeap<T> Right { get; private set; }
        public int Count { get; private set; }

        public void Add(T data)
        {
            if (Count == 0)
            {
                index = 0;
                Data = data;
                Root = this;
                Count++;
                array[0] = this;
                return;
            }
            if (this != Root)
            {
                Root.Add(data);
                return;
            }
            if (Count + 1 >= array.Length)
            {
                Array.Resize(ref array, array.Length * 2);
            }
            ComplexHeap<T> parent = array[(Count - 1) / 2];
            if (parent.Left == null)
            {
                parent.Left = new ComplexHeap<T>(parent.index * 2 + 1, data, this, parent);
                ReCount(parent, 1);
                BalancingBottomToTop(parent.Left);
            }
            else
            {
                parent.Right = new ComplexHeap<T>(parent.index * 2 + 2, data, this, parent);
                ReCount(parent, 1);
                BalancingBottomToTop(parent.Right);
            }
        }
        public bool Remove(T data)
        {
            if (Count == 0)
            {
                return false;
            }
            if (this != Root)
            {
                return Root.Remove(data);
            }
            ComplexHeap<T> required = Search(data);
            if (required == null)
            {
                return false;
            }
            if (Count == 1)
            {
                Clear();
                return true;
            }
            if (required == Root)
            {
                ComplexHeap<T> last = array[Count - 1];

                array[Count - 1] = null;

                required.Data = last.Data;

                ReCount(last.Parent, -last.Count);
                if (last == last.Parent.Left)
                    last.Parent.Left = null;
                else if (last == last.Parent.Right)
                    last.Parent.Right = null;

                last.index = 0;
                last.Data = default;
                last.Root = null;
                last.Parent = null;
                last.Left = null;
                last.Right = null;
                last.Count = 0;

                BalancingTopToBottom(Root);

                if (10 < Count && Count < array.Length / 2)
                    Array.Resize(ref array, array.Length / 2);

                return true;
            }
            else
            {
                ComplexHeap<T> last = array[Count - 1];

                array[required.index] = last;
                array[Count - 1] = null;

                ReCount(last.Parent, -last.Count);
                if (last == last.Parent.Left)
                    last.Parent.Left = null;
                else if (last == last.Parent.Right)
                    last.Parent.Right = null;

                if (required.Left != null)
                    required.Left.Parent = last;
                if (required.Right != null)
                    required.Right.Parent = last;
                if (required == required.Parent.Left)
                    required.Parent.Left = last;
                else if (required == required.Parent.Right)
                    required.Parent.Right = last;

                last.index = required.index;
                last.Root = required.Root;
                last.Parent = required.Parent;
                last.Left = required.Left;
                last.Right = required.Right;
                last.Count = required.Count;

                required.index = 0;
                required.Data = default;
                required.Root = null;
                required.Parent = null;
                required.Left = null;
                required.Right = null;
                required.Count = 0;

                BalancingTopToBottom(last);
                BalancingBottomToTop(last);

                if (10 < Count && Count < array.Length / 2)
                    Array.Resize(ref array, array.Length / 2);

                return true;
            }
        }
        public void Clear()
        {
            if (this != Root)
            {
                List<T> dataToRemove = new List<T>();
                foreach (ComplexHeap<T> heap in PostfixTraversal())
                {
                    dataToRemove.Add(heap.Data);
                }
                foreach (T data in dataToRemove)
                {
                    Root.Remove(data);
                }
            }
            else
            {
                foreach (ComplexHeap<T> heap in PostfixTraversal())
                {
                    heap.index = 0;
                    heap.Data = default;
                    heap.Root = null;
                    heap.Parent = null;
                    heap.Left = null;
                    heap.Right = null;
                    heap.Count = 0;
                }
                array = new ComplexHeap<T>[10];
            }
        }
        public ComplexHeap<T> Search(T data)
        {
            ComplexHeap<T> result = null;
            Search(data, ref result);
            return result;
        }
        public IEnumerable<ComplexHeap<T>> HorizontalTraversal()
        {
            Queue<ComplexHeap<T>> queue = new Queue<ComplexHeap<T>>();
            ComplexHeap<T> current = this;
            queue.Add(current);
            do
            {
                yield return queue.Remove();
                if (current.Left != null) queue.Add(current.Left);
                if (current.Right != null) queue.Add(current.Right);
                if (queue.Count > 0) current = queue.Peek();
            } while (queue.Count > 0);
        }
        public IEnumerable<ComplexHeap<T>> PrefixTraversal(List<ComplexHeap<T>> result = null)
        {
            if (result == null) result = new List<ComplexHeap<T>>();
            if (Count != 0) result.Add(this);
            Left?.PrefixTraversal(result);
            Right?.PrefixTraversal(result);
            return result;
        }
        public IEnumerable<ComplexHeap<T>> InfixTraversal(List<ComplexHeap<T>> result = null)
        {
            if (result == null) result = new List<ComplexHeap<T>>();
            Left?.InfixTraversal(result);
            if (Count != 0) result.Add(this);
            Right?.InfixTraversal(result);
            return result;
        }
        public IEnumerable<ComplexHeap<T>> PostfixTraversal(List<ComplexHeap<T>> result = null)
        {
            if (result == null) result = new List<ComplexHeap<T>>();
            Left?.PostfixTraversal(result);
            Right?.PostfixTraversal(result);
            if (Count != 0) result.Add(this);
            return result;
        }
        
        private void ReCount(ComplexHeap<T> start, int count)
        {
            ComplexHeap<T> current = start;
            while (current != null)
            {
                current.Count += count;
                current = current.Parent;
            }
        }
        private void BalancingBottomToTop(ComplexHeap<T> start)
        {
            ComplexHeap<T> current = start;
            while (current.Parent != null)
            {
                if (current.Data.CompareTo(current.Parent.Data) > 0)
                {
                    if (current.Parent != Root)
                    {
                        Swap(current, current.Parent);
                    }
                    else
                    {
                        (current.Data, current.Parent.Data) = (current.Parent.Data, current.Data);
                        current = current.Parent;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        private void BalancingTopToBottom(ComplexHeap<T> start)
        {
            ComplexHeap<T> current = start;
            ComplexHeap<T> max = null;
            while (current.Left != null)
            {
                if (current.Right == null)
                    max = current.Left;
                else
                    max = current.Left.Data.CompareTo(current.Right.Data) >= 0 ? current.Left : current.Right;

                if (current.Data.CompareTo(max.Data) < 0)
                {
                    if (current != Root)
                    {
                        Swap(max, current);
                    }
                    else
                    {
                        (max.Data, current.Data) = (current.Data, max.Data);
                        current = max;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        private void Swap(ComplexHeap<T> down, ComplexHeap<T> up)
        {
            (Root.array[down.index], Root.array[up.index]) = (Root.array[up.index], Root.array[down.index]);
            (down.index, up.index) = (up.index, down.index);

            ComplexHeap<T> downLeft = down.Left;
            ComplexHeap<T> downRight = down.Right;
            ComplexHeap<T> upParent = up.Parent;
            ComplexHeap<T> upLeft = up.Left;
            ComplexHeap<T> upRight = up.Right;

            down.Parent = up.Parent;

            if (down == up.Left)
            {
                down.Left = up;
                down.Right = up.Right;

                if (upRight != null)
                    upRight.Parent = down;
            }
            else if (down == up.Right)
            {
                down.Left = up.Left;
                down.Right = up;

                if (upLeft != null)
                    upLeft.Parent = down;
            }

            up.Parent = down;
            up.Left = downLeft;
            up.Right = downRight;

            if (downLeft != null)
                downLeft.Parent = up;
            if (downRight != null)
                downRight.Parent = up;

            if (up == upParent.Left)
                upParent.Left = down;
            else if (up == upParent.Right)
                upParent.Right = down;

            (down.Count, up.Count) = (up.Count, down.Count);
        }
        private void Search(T data, ref ComplexHeap<T> result)
        {
            if (Count != 0)
            {
                if (data.CompareTo(Root.array[Root.Count / 2].Data) == 0)
                {
                    result = Root.array[Root.Count / 2];
                }
                else if (data.CompareTo(Root.array[Root.Count / 2].Data) > 0)
                {
                    for (int i = 0; i <= Root.Count - 1; i++)
                    {
                        if (data.Equals(Root.array[i].Data))
                        {
                            result = Root.array[i];
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = Root.Count - 1; i >= 0; i--)
                    {
                        if (data.Equals(Root.array[i].Data))
                        {
                            result = Root.array[i];
                            break;
                        }
                    }
                }
            }
        }
    }
}