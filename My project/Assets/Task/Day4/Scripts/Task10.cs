using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;


public class Task10 : MonoBehaviour
{
    private int CalcSomeCount<T>(T num)
    {
        if(!double.TryParse(num.ToString(), out var d))
            throw new System.Exception("지원하지 않는 형입니다. {typeof(T).Name}");
        
        if(d < 0)
            d*= -1.0;
        
        var count = 0;

        while(1.0 < d)
        {
            ++count;
            d /= 2.0;
        }

        return count;
    }
    // public T CalcSomeCount<T>(T number)
    // {
    //     Type itemType = typeof(T);

    //     var tmp = (double)(object)number; //타입넣는방법?

    //     if (tmp < 0)
    //         tmp *= -1;

    //     int count = 0;

    //     while (1 < tmp)
    //     {
    //         ++count;
    //         tmp /= 2;
    //     }

    //     Debug.Log(count);

    //     return number;
    // }


    // List<string> typ = new List<string>() {"System.Double"};

    // public T CalcSomeCount<T>(T number) where T : struct, //where T : IComparable<T>
    // {
    //     Type itemType = typeof(T);
    //     bool hav = false;
    //     Debug.Log(itemType.ToString());

               
    //     foreach(string i in typ)
    //     {
    //         if(i == itemType.ToString())
    //         {
    //             number = (double)number; // ???
    //             if (number < 0)
    //                 number *= -1;

    //             int count = 0;

    //             while (1 < number)
    //             {
    //                 ++count;
    //                 number /= 2;
    //             }
    //         }
    //     }
    //     return number;
    // }
    
    

    // public T CalcSomeCount<T>(T number) where T : new()
    // {
    //     dynamic number = number;

    //     if (number < 0)
    //         number *= -1;

    //     int count = 0;

    //     while (1 < number)
    //     {
    //         ++count;
    //         number /= 2;
    //     }

    //     return number;
    // }

    void Start()
    {
        Debug.Log($"double : {CalcSomeCount<double>(10.0)}");
        Debug.Log($"float  : {CalcSomeCount<float>(-20.0f)}");
        Debug.Log($"int    : {CalcSomeCount<int>(13)}");
        Debug.Log($"string : {CalcSomeCount<string>("-123")}");
        Debug.Log($"string : {CalcSomeCount<string>("asdf")}");
    }
}
