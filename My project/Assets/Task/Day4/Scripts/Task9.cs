using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task9 : MonoBehaviour
{
    // 내부 클래스에서 사용하는 델리게이트 예시
    
    delegate void OnDoSomething(int i);

    OnDoSomething _onDoSomething = null;


    public void DoTest()
    {
        int tx = 2;
        if(tx == 1)
            _onDoSomething = DoAnything;
        else
            _onDoSomething = DoEverything;

        _onDoSomething?.Invoke(0);
    }

    private void DoAnything(int ak)
    {
        Debug.Log("anything");
    }

    private void DoEverything(int ak)
    {
        Debug.Log("Everything");
    }

    void Start()
    {
        DoTest();
    }

/*
    where T : class  = T는 참조 형식이어아한다. 
    where T : struct = T는 값 형식이어야한다.
    where T : new() = T는 반드시 매개변수가 없는 생성자가 있어야 한다.
    where T : 인터페이스명 = T는 명시한 인터페이스를 구현해야한다.
    where T : U = T는 또다른 형식 매개 변수 U로부터 상속받은 클래스어야 한다.
*/
}
