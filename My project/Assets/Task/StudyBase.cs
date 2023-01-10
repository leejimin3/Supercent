using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

[ExecuteAlways]
public abstract class StudyBase : MonoBehaviour
{
    [SerializeField] bool doLog = false;

    protected abstract void OnLog();

    void Awake()
    {
        Reset();
    }

    void Reset()
    {
        ClearLog();
        OnLog();
    }
    void OnValidate()
    {
        if (doLog)
        {
            doLog = false;
            Reset();
        }
    }

    protected static void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }

    protected static void Log(object message) => Debug.Log(message);


    protected static int[] GetRandArray(int count, int? seed = null)
    {
        if (count < 0)
            return new int[0];

        var arrRand = new int[count];
        for (int index = 0; index < arrRand.Length; ++index)
            arrRand[index] = index;

        if (seed.HasValue)
            UnityRandom.InitState(seed.Value);

        int halfIndex = arrRand.Length >> 1;
        int tmp;
        for (int no = 0; no < 5; ++no)
        {
            for (int index = 0; index < halfIndex; ++index)
            {
                var randIndex = UnityRandom.Range(0, halfIndex) + halfIndex;
                tmp = arrRand[index];
                arrRand[index] = arrRand[randIndex];
                arrRand[randIndex] = tmp;
            }
        }

        return arrRand;
    }
}


public static class StudyExtensions
{
    static readonly StringBuilder sb = new StringBuilder();

    static void LogJob(int count, Type type)
    {
        if (0 < sb.Length)
        {
            sb[0] = ']';
            sb.Insert(0, '[')
              .Insert(1, count);
        }
        else
        {
            sb.Append('[')
              .Append(count)
              .Append(']');
        }

        sb.Insert(0, $"{type.Namespace}.{type.Name} : ");

        Debug.Log(sb.ToString());
    }

    public static void LogValues<T>(this IEnumerable<T> src)
    {
        sb.Length = 0;
        int count = 0;
        foreach (var value in src)
        {
            ++count;
            sb.Append(", ")
              .Append(value.ToString());
        }

        LogJob(count, src.GetType());
        sb.Length = 0;
    }

    public static void LogValues(this IEnumerable src)
    {
        sb.Length = 0;
        int count = 0;
        foreach (var value in src)
        {
            ++count;
            sb.Append(", ")
              .Append(value.ToString());
        }

        LogJob(count, src.GetType());
        sb.Length = 0;
    }

    public static void LogValues<T>(this IEnumerator<T> src)
    {
        sb.Length = 0;
        int count = 0;

        while (src.MoveNext())
        {
            ++count;
            sb.Append(", ")
              .Append(src.Current.ToString());
        }

        LogJob(count, src.GetType());
        sb.Length = 0;
    }

    public static void LogValues<T>(this IEnumerator src)
    {
        sb.Length = 0;
        int count = 0;

        while (src.MoveNext())
        {
            ++count;
            sb.Append(", ")
              .Append(src.Current.ToString());
        }

        LogJob(count, src.GetType());
        sb.Length = 0;
    }
}