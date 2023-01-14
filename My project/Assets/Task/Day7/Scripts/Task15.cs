using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public sealed class Dictionary<K, T> : IEnumerable<KeyValuePair<K, T>>
{   
    // next는 버킷에 겹쳐있는 value를 순서대로 찾아주는 역할을 함.(비둘기 집)
    public const int MaximumCount = 1000;       // 버켓 부와 딕셔너리 부를 분리한 뒤 딕셔너리 부의 리사이즈 코드를 작성

    // buckets, entries 구현이 필요합니다.

    public int Count { private set; get; } = 0;
    private List<int> list = new List<int>();    //키 코드를 모아놓는 리스트    >>> 리스트를 만드는 것보다 for문을 도는게 나았다. And 이게 버켓의 역할이니 굳이 리스트를 만들 필요가 없었다.          Issue .. : 제네릭이 아님.

    int[] buckets = new int[MaximumCount];
    Entry[] entries = new Entry[MaximumCount];


    public T this[K key]
    {
        //예외 처리
        set
        {
            ADDjob(key, value);
        }

        get
        {   //key가 존재하지 않는 경우의 예외처리를 해줘야함.
                // var entryIndex = FindEntryIndex(key);
                // if (entryIndex < 1)
                //     throw new Exception($"{key}는 존재하지 않는 키값 입니다");

            return entries[key.GetHashCode()].value;    //value를 리턴. 키값을 출력하면 밸류값이 나오기 때문
        }
    }

    public bool ADDjob(K key, T value)  //ADD와 생성자의 작동을 통일.
    {
        int KeyCode = key.GetHashCode();

        buckets[KeyCode] = KeyCode;     // Buckets에 키코드 대입

        list.Add(KeyCode);



        entries[KeyCode].hashCode = KeyCode;

        if((int)(object)entries[KeyCode].key == 0)      //Comparer 활용하기, 해쉬코드는 0이 나올 수 있음
                entries[KeyCode].next = -1;
        else
                entries[KeyCode].next = entries[KeyCode].hashCode;

        entries[KeyCode].key = key;
        entries[KeyCode].value = value;
        
        Count++;

        return true;    //ADD의 반환값
    }

    public bool ContainsKey(K key)      //Entries[key.GetHashCode]의 값이 0이 아니라면 true
    {
        int hash = key.GetHashCode();
        
        if(entries[hash].key.Equals(key))
                return true;

        return false;
    }
    public bool ContainsValue(T value)          //리스트에 담아놓은 키가 0이 아니고 그것의 value값이 입력된 것과 같으면 true 리턴
    {
        foreach(int i in list.Distinct().ToList())
        {       
                if((int)(object)entries[i].key != 0 && entries[i].value.Equals(value) == true)
                        return true;
        }
        return false;
    }

    public bool TryGetValue(K key, out T result)        //해쉬가 null이 아니라면 존재하는 것이고 value를 바궈준뒤 true리턴
    {
        int hash = key.GetHashCode();

        if(hash != 0)
        {
                result = entries[hash].value;
                return true;
        }

        result = default;
        return false;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();         //리스트에서 인덱스 값을 받아와 출력
    public IEnumerator<KeyValuePair<K, T>> GetEnumerator()
    {
        foreach(int i in list.Distinct().ToList())
        {
                if((int)(object)entries[i].key != 0)
                        yield return new KeyValuePair<K, T>(entries[i].key, entries[i].value);
        }
    }


    public bool Add(K key, T value)     //bool값 반환할 것, 반환값이 다른이유를 고민해보기      == Add는 실패할 수 있기 때문???
    {
        return ADDjob(key, value);
    }


    public bool Remove(K key)   //해당 엔트리의 모든 값을 default로 변경 >> next를 넘겨주는 작업을 안함.
    {
        int hash = key.GetHashCode();
        if(hash != 0)
        {
                entries[hash].hashCode = default;
                entries[hash].next = default;
                entries[hash].key = default;
                entries[hash].value = default;
                Count--;
                return true;
        }   
        
        return false;
    }

    public void Clear()         //list에 들어있는 인덱스 삭제
    {

        foreach(int i in list.Distinct().ToList())
        {
                if((int)(object)entries[i].key != 0)
                       Remove(entries[i].key);
        }

        Count = 0;
    }


    struct Entry        // 구조체에서도 생성자와 Set, Get을 선언할 수 았음. But 생성자를 선언할 때는 모든 멤버의 값을 할당해줘야만 함
    {
        public int hashCode;
        public int next;
        public K key;
        public T value;
    }
}

public class Task15 : StudyBase
{
        protected override void OnLog()
        {
                // Hash는 알고리즘 마다 저장 순서가 다를수 있습니다
                var map = new Dictionary<int, string>();
                string tmp = "";

                map[101] = "김민준";
                map[201] = "윤서준";
                map[101] = "박민준";
                map.LogValues();

                // [101, 박민준], [201, 윤서준]
                map.Add(302, "김도윤");
                map.Remove(101);
                map.Add(102, "서예준");
                // // [102, 서예준], [201, 윤서준], [302, 김도윤]
                map.LogValues();

                
                Debug.Log("ContainsKey");
                Debug.Log("102번 검색 : " + map.ContainsKey(102)+ ", 104번 검색 : " + map.ContainsKey(104));
                
                Debug.Log("ContainsValue");
                Debug.Log("서예준 검색 : " + map.ContainsValue("서예준") + ", 서예진 검색 : " + map.ContainsValue("서예진"));
                
                Debug.Log("TryGetValue");
                Debug.Log(map.TryGetValue(102, out tmp) +", " + tmp);
        }
}