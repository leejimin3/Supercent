using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public sealed class List<T> : IEnumerable<T>
{
    int DefaultCount = 4;
    T[] Array = new T[0];
    public int Count { private set; get; } = 0;

    public T this[int index]
    {
        set
        {
            Array[index] = value;
        }
        get
        {
            return Array[index];
        }
    }

    public List()
    {
        Array = new T[DefaultCount];
    }

    public bool Contains(T value)
    {
        foreach (T i in Array)
        {
            if (i.Equals(value))
                return true;
        }
        return false;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < Array.Length; i++)
        {
            yield return Array[i];
        }
    }

    public void Add(T value)
    {
        UpdateCapacity();
        Array[Count] = value;
        Count++;
    }

    public void Insert(int index, T value)
    {
        UpdateCapacity();

        for (int i = Count; i > index; i--)
        {
            Array[i] = Array[i - 1];
        }

        Count++;
        Array[index] = value;

    }

    public bool Remove(T value)
    {
        for (int i = 0; i < Count; i++)
        {
            if (Array[i].Equals(value) == true)
            {
                Array[i] = default;
                for (int j = i; j < Count - 1; j++)
                    Array[j] = Array[j + 1];

                Array[Count - 1] = default;

                Count--;
                return true;
            }
        }
        return false;
    }

    public void RemoveAt(int index)
    {
        Array[index] = default;
        for (int i = index; i < Count; i++)
        {
            Array[index] = Array[index + 1];
        }
        Array[Count - 1] = default;

        Count--;
    }

    public void Clear()
    {
        for (int i = 0; i < Array.Length; i++)
        {
            if (Array[i] == null)
                break;

            Array[i] = default;
        }

        Count = 0;
    }

    void UpdateCapacity()
    {
        if (Array.Length == Count)
        {
            T[] newArray = new T[Array.Length * 2];

            for (int i = 0; i < Array.Length; i++)
            {
                newArray[i] = Array[i];
            }

            Array = newArray;
        }
    }
}

public class Task12 : StudyBase
{

    protected override void OnLog()
    {
        var aList = new List<int>();

        aList.Add(2);
        // 2
        aList.LogValues();

        aList.Insert(0, 1);
        // 1, 2
        aList.LogValues();

        aList.Add(4);
        aList.Insert(aList.Count - 1, 3);
        // 1, 2, 3, 4
        aList.LogValues();

        aList.Remove(2);
        aList.RemoveAt(0);
        // 4
        Log(aList[aList.Count - 1]);
    }
}