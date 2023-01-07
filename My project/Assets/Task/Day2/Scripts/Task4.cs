using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Task4 : MonoBehaviour
{
    // Issue .. : 컴파일이 실행되지 않아 String으로 넣어 유니티로 출력했지만 String남용으로 비효율적인 코드라고 판단.
    
    void Start()
    {
        int[] list = new int[] {1,2,3,4,5,6,7,8,9};  // 화면에 표시할 숫자 배열 생성

        for(int i = 1; i <= list.Length; i++) // Loop는 1부터 시작. 이유는 반복문의 조건이 0<0이 돼서 실행이 안되는 것을 방지하기 위해. 
        {
            string tmp = "";
            for(int j = 0; j < list.Length; j++)  // 앞에서부터 출력. j<9 로 설정한 이유는 공백을 출력하기 위함.
            {
                if(j < i)
                    tmp = tmp + list[j];
                else
                    tmp = tmp + "-";
            }
            Debug.Log(tmp);
            tmp = "";
            
            
            if(i == list.Length) // 마지막 줄을 한줄로 출력하기 위함.          To do .. : 조건문을 돌때마다 조건을 검사하여 효율적이지 못함. 리팩토링 필요.
                return;


            for(int j = 0; j < list.Length ;j++) // 뒤에서부터 출력
            {
                if(j >= list.Length-i)               // '>='로 한 이유는 비교값이 -1이 되는 것을 방지하기 위함.    9-i는 출력해도 되는 위치를 나타내는 값
                    tmp = tmp + list[j];
                else
                    tmp = tmp + "-";
            }
            Debug.Log(tmp);
            Console.WriteLine("gd");
        }
        
    }

}
