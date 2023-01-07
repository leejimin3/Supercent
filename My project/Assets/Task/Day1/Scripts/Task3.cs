using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task3 : MonoBehaviour
{
    void Start()
    {   
        string a = "Hello World!";
        Debug.Log(a);
        Debug.Log(GC.GetTotalMemory(false)); // Hello World 출력후 메모리 탐색

        a = a.Substring(0,11);
        Debug.Log(a);
        Debug.Log(GC.GetTotalMemory(false)); // 뒤에 ! 제거 후 메모리 탐색

        a = a+ "!";
        Debug.Log(a);
        Debug.Log(GC.GetTotalMemory(false)); // 뒤에 ! 추가 후 메모리 탐색

        
        ////    Think .. : GC의 메모리 탐색에서 결과가 같게 나오는 이유는 변화가 일어난 string의 값이 GC로 들어가지 않기 때문이라고 생각.

    }
}
