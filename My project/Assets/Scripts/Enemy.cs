using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // 애니메이션 파라미터
    static readonly int ANIM_Hit = Animator.StringToHash("Hit");

    // 체력
    [Header("Health")]
    [SerializeField] float MaxHealth = 15.0f;   // 최대 체력
    [SerializeField] float CurrentHealth = 15.0f; // 현재 체력
    
    // 체력 바
    [Space]
    [Header("Hpbar")]
    [SerializeField] GameObject canvas; // 슬라이더 캔버스
    [SerializeField] Slider hpslider; // hp슬라이더
    [SerializeField] Slider afterslider; // 줄어드는 이펙트 슬라이더

    // 래그돌
    [Space]
    [Header("Ragdoll")]
    [SerializeField] Collider Thiscol; // 콜라이더
    [SerializeField] Rigidbody ThisRig; // 리지드바디
    [SerializeField] Rigidbody knockbackbone = null; // 죽었을 때 날아갈 본
    [SerializeField] float KnockBackPower = 1000.0f; // 넉백 높이 & 파워
    Collider[] col = null;  // 래그돌 콜라이더
    Rigidbody[] rig = null;    // 래그돌 리지드바디

    //사운드
    [Space]
    [Header("Sound")]
    [SerializeField] AudioClip BoomSound = null;
    [SerializeField] AudioClip BloodSound = null;

    // 애니매이터
    [Space]
    [Header("Animator")]
    [SerializeField] Animator Anim; // 애니메이터

    // 파티클
    [Space]
    [Header("Particles")]
    [SerializeField] GameObject blood; // 초록 피 이펙트
    [SerializeField] GameObject boom; // 작은 폭발 이펙트

    // 스파인
    [Space]
    [Header("Spine")]
    [SerializeField] private Transform spine; // 상체 하체 분리 기준 bone (허리)

    
    void Awake()
    {
        //초기화
        CurrentHealth = MaxHealth;
        GetRagdollBits();
        SetRagdoll(false);
    }

    void GetRagdollBits()   // 래그돌의 콜라이더와 리지드바디를 배열에 넣음
    {
        col = transform.Find("Armature").GetComponentsInChildren<Collider>();
        rig = transform.Find("Armature").GetComponentsInChildren<Rigidbody>();
    }

    void SetRagdoll(bool act) // act가 true면 래그돌 활성화, false면 비활성화
    {
        Anim.enabled = !act;
        ThisRig.isKinematic = act;
        Thiscol.isTrigger = act;


        for(int i = 0, cnt = rig.Length; i < cnt; i++)
        {
            var rigid = rig[i];
            if(null == rigid)
                continue;

            rigid.isKinematic = !act;
        }

        for(int i = 0, cnt = col.Length; i < cnt; i++)
        {
            var collider = col[i];
            if(null == collider)
                continue;

            collider.isTrigger = !act;
        }
    }


    IEnumerator SliderCoroutine() //첫 슬라이더가 감소하고 두번째 슬라이더는 0.5초후에 따라오도록 설정(Invoke)
    {
        while(hpslider.value - CurrentHealth / MaxHealth > 0.001f)
        {
            hpslider.value = Mathf.Lerp(hpslider.value, CurrentHealth / MaxHealth, Time.deltaTime * 5f);
            Invoke("aftersliderRoutine", 0.5f);
            yield return null;
        }
    }
    void aftersliderRoutine()
    {
        afterslider.value = Mathf.Lerp(afterslider.value, CurrentHealth / MaxHealth, Time.deltaTime * 5f);
    }


    public bool TakeDamage(float damage, GameObject obj) // 데미지 처리
    {

        canvas.SetActive(true); // 슬라이더 캔버스 활성화
        CurrentHealth -= damage; // 데미지
        Anim.SetTrigger(ANIM_Hit); // 애니메이션 파라미터
        HitParticle(obj.transform.position, spine.position); // 파티클 함수 호출
        StartCoroutine(SliderCoroutine()); // 피 감소 코루틴
        
        if (CurrentHealth <= 0f) // 마지막 일격으로 죽었으면
        {
            SetRagdoll(true); // 래그돌 활성화
            GameManager.instance.EnemyDie(); // 게임매니저에서 함수호출
            GetComponent<EnemyNav>().Isdie = true; // 네비게이션 클래스에 신호
            this.tag = "Untagged"; // 태그 없애기(계속 Tracking 인식됨)
            var dir = transform.position - obj.transform.position; // 방향벡터
            knockbackbone.AddForce(dir * KnockBackPower + Vector3.up * 3f * KnockBackPower); // 방향벡터의 방향으로 KnockBackPower의 힘으로 날려줌 (3f는 높이)
            Invoke("CanvasClose", 2.0f);    // 2초 뒤에 슬라이더 없어짐
            Invoke("SinkEnemy", 3.0f); // 3초 뒤에 플레이어 가라앉음
            
            return true;
        }

        Invoke("CanvasClose", 2.0f); // 죽지 않았다면 캔버스만 2초뒤에 닫아줌

        return false;
    }

    void HitParticle(Vector3 _player, Vector3 _enemy)   //파티클 재생부
    {
        var rot = Quaternion.LookRotation(_player - _enemy);    // 방향을 LookRotation으로 받음
        var greenblood = Instantiate(blood , spine.position, rot);
        var bulletfire = Instantiate(boom , spine.position, rot);
        
        greenblood.GetComponent<ParticleSystem>().Play();
        SoundManager.instance.PlayAudio("BloodSound", BloodSound);
        
        boom.GetComponent<ParticleSystem>().Play();
        SoundManager.instance.PlayAudio("BoomSound", BoomSound);
    }



    //Invoke함수 호출 부
    void CanvasClose() { canvas.SetActive(false); }
    void SinkEnemy()    //래그돌의 리지드바디 키네마틱은 꺼주고 콜라이더의 트리거는 켜줘서 Plane과 충돌하지 않게끔 하여 Sink구현
    {
        for (int i = 0, cnt = rig.Length; i < cnt; i++)
        {
            var rigid = rig[i];
            if (null == rigid)
                continue;

            rigid.isKinematic = false;
        }

        for (int i = 0, cnt = col.Length; i < cnt; i++)
        {
            var collider = col[i];
            if (null == collider)
                continue;

            collider.isTrigger = true;
        }
        Destroy(this.gameObject, 2.0f); // Sink된 뒤 2초뒤 파괴
    }
}