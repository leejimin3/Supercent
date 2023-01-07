using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;



public class Task2 : MonoBehaviour
{
    void CheckfractionLeftPoint(float fnum, ref StringBuilder list) // MoveP 는 소수점 이동 자리를 반환    
    {
        int num = (int)fnum;

        Stack<int> frontstack = new Stack<int>(); //Stack을 사용하여 이진수화 진행. Stack은 LIFO이기 때문에 이진수화에 용이한 배열이라 판단.

        while(num > 0.5) // 정수부분 이진수 화 (조건을 '> 0'으로 할 시 Issue가능성이 있어 0.5로 설정)
        {
            if(num % 2 == 1)
                frontstack.Push(1);
            else
                frontstack.Push(0);

            num /= 2;
        }

        while(frontstack.Count != 0)
        {
            var tmp = frontstack.Pop();

            list.Append(tmp);
        }
    }

    
    void CheckfractionRightPoint(float fnum, ref StringBuilder Gasu)
    {
        int num = (int)((fnum - (int)fnum)*1000); //전체에서 정수부분만 제거하여 소수부분 추출하고 1000을 곱해 정수로 만들어 float문제 방지
        
        while(num != 1000)      // 값이 1000이 될때까지 String에 Append
        {
            num *= 2;

            Gasu.Append(num/1000);
            if(num > 1000) num -= 1000;
        }
    }

    void Start()
    {   
        float[] NumList = {13.5f, -27.125f, 0.125f, -0.75f}; //NumList에 과제의 수 4개 삽입
        
        foreach (float numlist in NumList)
        {
            var Point = 0;
            var Bit = new StringBuilder(); //stringbuilder를 사용하면 스트링을 수정해도 메모리 누수가 발생하지 않음
            var Jisu = new StringBuilder(); //지수부분 임시저장
            var Gasu = new StringBuilder(); //가수부분 임시저장


            if(numlist > 0)
                Bit.Append("0");
            else
                Bit.Append("1");

            float absnum = Math.Abs(numlist); // 절대값


            CheckfractionLeftPoint(absnum, ref Gasu); // 가수 비트 정수 부분 체크 & 삽입
            Point = Gasu.Length;    //정수 부분의 자릿수 저장

            CheckfractionRightPoint(absnum, ref Gasu); // 가수 비트 소수 부분 체크 & 삽입


            var tmp = 1;
            for(int i = 0; i < Gasu.Length; i++)    //포인트 자릿수 계산
            {
                if(Gasu[i] == '1')
                    break;
                tmp++;
            }

            Point -= tmp;               //새로운 포인트 계산후 지수 추출 후 제거
            Gasu.Remove(0 , tmp);

            for(int i = 0, cnt = 23 - Gasu.Length; i < cnt; i++) { Gasu.Append(0); }    //나머지 부분 0으로 채우기 (23 = 가수부분 전체 수)

            CheckfractionLeftPoint(Point+127, ref Jisu); // 지수 비트 체크 & 변경

            
            if(Jisu.Length == 7)        //지수의 앞자리가 0이라면 추가
                Jisu.Insert(0, "0");

            Debug.Log($"숫자 : {numlist},  변환 : {Bit}{Jisu}{Gasu}");
        }
    }
}
