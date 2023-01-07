using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Market : MonoBehaviour
{

    Mesh mesh; //메시

    protected abstract void Touch(); //교체
    protected abstract void sell(); //판매
    void Dummy()
    {
        // 완판 시 더미 노출
    }

    void Data()
    {
        // 데이터 보존
    }

}


public class WeaponMarket : Market
{

    protected override void Touch()
    {
        //조건에 맞는 버튼 출력
    }
    protected override void sell()
    {
        //장착 버튼 출력
    } 
}

public class SkinMarket : Market
{
    //장착, 교환 버튼
    protected override void Touch()
    {
        //구매한 항목이라면 즉시 착용
    }
    protected override void sell()
    {
        //완료 시 바로 착용
    } 
}
