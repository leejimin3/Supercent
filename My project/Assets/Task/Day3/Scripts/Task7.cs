using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Task7 : MonoBehaviour  //To do .. : 예외처리
{   
    public string Name = "";
    //리스트 선언 부
    private List<string> Mlist = new List<string>() {"스타워커", "루루", "카로", "쿠퍼", "리아"};   //멤버 리스트 Mlist
    private List<int> Llist = new List<int>() {1, 2, 3, 4, 5}; //번호리스트 Nlist

    private List<List<string> > list = new List<List<string> >() {  //목록리스트 list
        new List<string>() {"순두부", "피자", "햄버거", "스파게티"},
        new List<string>() {"openGL", "c#", "math"},
        new List<string>() {"CoinHUD", "IStackable"},
        new List<string>() {"파워디그"},
        new List<string>() {"UI", "2D", "Sprite", "Texture"}
    };
    //


    //딕셔너리 선언부
    private Dictionary<string, List<string> > listdict = new Dictionary<string, List<string> >();
    private Dictionary<string, int> numdict = new Dictionary<string, int>();
    //


    //생성자에 딕셔너리 값 입력
    private Task7()
    {
        for(int i = 0; i<list.Count; i++) { listdict.Add(Mlist[i], list[i]); }
        for(int i = 0; i<list.Count; i++) { numdict.Add(Mlist[i], Llist[i]); }
    }

    private void Start()
    {
        if(Mlist.Find(tmp => tmp == Name) == null)
        {
            Debug.Log("'" + Name + "' 라는 사람이 목록에 없습니다.");
            return;
        }
            
        Find(Name);
    }

    private void Find(string name)
    {
        var str = new StringBuilder();

        if(!numdict.TryGetValue(name, out var number))
            number = int.MinValue;
        if(!listdict.TryGetValue(name, out var list))
            list = null ;
        
        for(int i = 0; i < this.list[number-1].Count; i++) 
        { 
            if(0<i)
                str.Append(", ");
            str.Append(list[i]);
        }
        
        Debug.Log(name + "님의 번호 : " + number + "\n" + name + "님의 목록 : " + str);
        
    }
}
