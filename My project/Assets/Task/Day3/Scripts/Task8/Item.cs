using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    string itemname; //이름
    Mesh mesh; //외형
}

public abstract class EqualItem : Item
{
    string grade; //등급
    int info; //구매정보

    protected abstract void BuyEvent();
    protected abstract void TouchEvent();

    void CheckMoney()
    {
        //금액 확인 후 있으면 구매, 없으면 광고 제의
    }

    void Adv()
    {
        //광고 시청 이벤트 호출
    }

    void Equip()
    {
        //착용
    }
}


public class SkinItem : EqualItem
{

    int level; //레벨
    protected override void BuyEvent()
    {
        //착용
    }

    protected override void TouchEvent()
    {
        //터치시 구매한 아이템이라면 착용
    }
}

public class WeaponItem : EqualItem
{
    protected override void BuyEvent()
    {
        //장착 버튼 호출
    }

    protected override void TouchEvent()
    {
        //장착 버튼, 구매 버튼 이벤트 호출
    }
}