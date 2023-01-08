using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task5 : MonoBehaviour
{


    






    // Sin함수를 사용하지 않은 코드

    // void SetInitialization(bool[,] box)     //2차원 bool 배열 초기화
    // {
    //     for(int i = 0; i < box.GetLength(0); i++)
    //     {
    //         for(int j = 0; j < box.GetLength(1); j++)
    //         {
    //             box[i, j] = false;
    //         }
    //     }
    // }

    // void SetDot(bool[,] box, int[][] key)
    // {
    //     for(int i = 0; i < box.GetLength(0)/2+1; i++)      //위에서 6번, 아래에서 6번 반복
    //     {
    //         foreach(int idx in key[i])
    //         {
    //             box[i, idx] = true;  //위에서 키 값에 해당하는 인덱스 값 변경
    //             box[(box.GetLength(0)-1)-i, (box.GetLength(1)-1)-idx] = true;  //아래에서 최댓값 - 키값에 해당하는 인덱스 값 변경
    //         }
    //     }
    //         // for(int k = 0 ; k < 20; k++)
    //         // {
    //         //     Debug.Log(box[5, k]);
    //         // }
    // }
    
    // void ShowDot(bool[,] box, int[][] key)
    // {
    //     string tmp = "";
    //     for(int i = 0; i < box.GetLength(0); i++)
    //     {
    //         for(int j = 0; j<box.GetLength(1); j++)     //흰색이면 W, 검정색이면 B를 출력
    //         {
    //             if(box[i, j] == true)
    //                 tmp = tmp + "□";
    //             else
    //                 tmp = tmp + "■";
    //         }

    //         Debug.Log(tmp);     // tmp 출력 후 초기화
    //         tmp = "";
    //     }
    // }
    void Start()
    {







        // Sin함수를 이용하지 않은 구현법
        
        // int[][] key = { new int[] {4, 5, 6}, new int[] {3, 7}, new int[] {2}, 
        //                 new int[] {1, 8}, new int[] {9}, new int[] {0} }; // 가변배열 생성

        // bool[,] box = new bool[11, 20]; // 도트를 표현하기 위한 2차원 배열

        // SetInitialization(box); // dot배열을 false로 초기화
        // SetDot(box, key); // key값을 토대로 box 변경
        // ShowDot(box, key); // key값을 출력
    }
}
