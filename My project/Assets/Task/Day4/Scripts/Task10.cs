using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Task10 : MonoBehaviour
{

    public T CalcSomeCount<T>(T number)
    {
        Type itemType = typeof(T);

        var tmp = (double)(object)number; //타입넣는방법?

        if (tmp < 0)
            tmp *= -1;

        int count = 0;

        while (1 < tmp)
        {
            ++count;
            tmp /= 2;
        }

        Debug.Log(count);

        return number;
    }


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
        CalcSomeCount((double)10.0);
        //Debug.Log(CalcSomeCount((float)-20.0f));
        //Debug.Log(CalcSomeCount((int)13));
        //Debug.Log(CalcSomeCount((string)"-123"));
        //Debug.Log(CalcSomeCount((string)"asdf"));
    }
}
