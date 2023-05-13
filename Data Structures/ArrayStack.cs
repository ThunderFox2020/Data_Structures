using System;
using System.Collections.Generic;

public class ArrayStack<T>
{
    public ArrayStack(int length = 10)
    {
        array = new T[length];
        minLength = length;
    }

    private T[] array;
    private int minLength;

    public int Count { get; private set; }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = Count - 1; i >= 0; i--)
        {
            yield return array[i];
        }
    }
    public void Add(T data)
    {
        if (Count >= array.Length)
        {
            Array.Resize(ref array, array.Length + (array.Length / 2));
        }
        array[Count++] = data;
    }
    public T Remove()
    {
        if (minLength < Count && Count < array.Length / 2)
        {
            Array.Resize(ref array, array.Length - (array.Length / 2));
        }
        if (Count > 0)
        {
            T value = array[--Count];
            array[Count] = default;
            return value;
        }
        else
        {
            throw new Exception("The stack is empty");
        }
    }
    public void Clear()
    {
        Array.Clear(array, 0, array.Length);
        Array.Resize(ref array, minLength);
        Count = 0;
    }
    public T Peek()
    {
        if (Count > 0)
        {
            return array[Count - 1];
        }
        else
        {
            throw new Exception("The stack is empty");
        }
    }
}