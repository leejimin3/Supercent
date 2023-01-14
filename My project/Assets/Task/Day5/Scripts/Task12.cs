using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public sealed class List<T> : IEnumerable<T>
{
    int DefaultCount = 4;                       //기본 카운트를 4로 설정한다. 이유는 리스트가 생성될때 기본으로 4의 공간을 할당하기 때문
    T[] Array = new T[0];
    public int Count { private set; get; } = 0;

    public T this[int index]        //set, get에 index가 0보다 작을 시와 Count보다 클 경우의 예외처리를 해야한다.
    {
        set
        {
            // if (index < 0)                                           //Set, Get에 들어갔어야 할 예외처리
            //     throw new Exception("index가 0보다 작습니다");
            // if (Count <= index)
            //     throw new Exception($"index가 {Count}보다 크거나 같습니다");

            Array[index] = value;           //Ex) Array[4] = 4 로 설정했을 때 set을 통해 값 set
        }
        get
        {
            return Array[index];            //Debug.Log(list[4])를 입력받을 시 get을 통해 값 전달
        }
    }

    public List()
    {
        Array = new T[DefaultCount];        //리스트는 제네릭형 배열로 구성되어 있음
    }

    public bool Contains(T value)           // 원하는 값이 리스트에 들어가 있는지 확인 후 반환
    {
        foreach (T i in Array)
        {
            if (i.Equals(value))
                return true;
        }
        return false;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();     //지정해 놓은 Ienumerable을 통해 값 출력

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < Count; i++)   // 이 부분을 Array.Length까지로 선언했었는데 그러면 값이 배열의 총 크기만큼 출력됨. 우리가 원하는 건 할당된 배엶만 출력하는 것이므로 Count로 하는게 맞음
        {
            yield return Array[i];
        }
    }

    public void Add(T value)        //Capacity를 재조정해야하는지 확인 후 값 추가
    {
        UpdateCapacity();
        Array[Count] = value;
        Count++;
    }

    public void Insert(int index, T value)  //Capacity재조정을 확인 후 값 삽입
    {
        UpdateCapacity();

        for (int i = Count; i > index; i--)
        {
            Array[i] = Array[i - 1];
        }

        Count++;
        Array[index] = value;

    }

    public bool Remove(T value)                 //값을 찾고 있다면 없애고 배열 당겨오기
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

    void UpdateCapacity()           //Capacity2배로 증가
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