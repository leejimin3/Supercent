using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Answer5 : MonoBehaviour
{
    // To do ,, : 디버그 진행하며 값 변화 확인하기
    void Start()
    {
        for (int y = 0; y < 11; ++y)
        {
            for (int x = 0; x < 20; ++x)
            {
                var d = (double)x / 19;
                d *= Math.PI;
                d *= 2.0;

                var sin = Math.Sin(d);              //2파이는 x축의 길이가 아니라 싸인의 주기
                var yy = (sin * 5.0 + 5.0);         //5를 곱한건 최대치의 길이를 곱한것(높이) 이고 더한 것은 Round함수를 편하게 쓰기 위해 양수로 바꿔준 것
                var yyy = 10.0 - Math.Round(yy, 0);

                if (y == yyy)
                    Debug.Log("□");
                else
                    Debug.Log("■");
            }
        }
    }
}
