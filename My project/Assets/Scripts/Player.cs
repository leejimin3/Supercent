using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //애니메이터 파라미터
    static readonly int ANIM_IDLE = Animator.StringToHash("isIdle");
    static readonly int ANIM_MOVE = Animator.StringToHash("isMove");
    static readonly int ANIM_SHOOT = Animator.StringToHash("isShoot");
    static readonly int ANIM_RELOAD = Animator.StringToHash("isReload");
    static readonly int ANIM_RANDOMINT = Animator.StringToHash("RandomInt");
    static readonly int ANIM_MOVE_SPEED = Animator.StringToHash("Blend");

    //메인 카메라 >> 카메라 쉐이크 호출
    [Space]
    [Header("Camera")]
    [SerializeField] Camera camera = null;

    [Space]
    [Header("Animator")]
    [SerializeField] Animator Anim;

    //걷기(Walk) 블렌더 커브와 값
    [Space]
    [Header("Animation Blend")]
    [SerializeField] AnimationCurve MoveCurve = null;
    [SerializeField] float MoveAccelTime = 1.0f;

    [Space]
    [Header("Audio")]
    [SerializeField] AudioClip ShotSound = null;
    [SerializeField] AudioClip ReloadSound = null;

    //이동 관련
    [Space]
    [Header("Movement")]
    [SerializeField] float Speed = 7.5f;    // 기본 이동 속도
    [SerializeField] float CanMove = 10.0f; // 최소 이동 거리(_moveThreshold) >> 마우스를 최소한 움직여야 하는 정도
    Vector3 MouseDownPos; // 처음 터치되는 자리
    float MoveStart = 0.0f; // 이동이 처음 시작되는 시간  >> IdleAction체크를 위함
    float MoveStop = 0.0f; // 이동이 끝나는 시간
    float lastIdleActionAt = 0.0f; // 마지막 대기 모션 시간
    float lastShootAt = 0.0f;  // 마지막 사격 시간
    
    // 사격 관련
    [Space]
    [Header("Bullet")]
    [SerializeField] public int MaxBullet = 7;     // 캐릭터 최대 총알 개수
    [SerializeField] public int CurrentBullet = 7; // 현재 총알 개수
    [SerializeField] float Range = 5.0f; // 사거리
    GameObject targetenemy = null; // 에임중인 타겟
    bool isAim = false; // 캐릭터가 에임중인지 확인하는 파라미터

    // 쿨타임 관련
    [Space]
    [Header("Cooldown")]
    [SerializeField] float IdleActionInterval = 3.0f; // 대기 모션 쿨타임 (애니메이션이 끝나는 시간 기준)
    [SerializeField] float ShootCoolTime = 1.2f; // 사격 쿨타임 (발사와 발사 사이 간격)

    // 파티클
    [Space]
    [Header("Particles")]
    [SerializeField] GameObject Rayzer; // 레이저 파티클

    //Spine뼈대
    [Space]
    [Header("Spine")]
    [SerializeField] private Transform spine; // 상체 하체 분리 기준 bone (허리)

    //초기화 부분. 생성자처럼 사용 >> Awake에서 초기화 시 인스펙터에서 값을 설정해도 다시 초기화
    void Awake()
    {
        CurrentBullet = MaxBullet;
        Anim.SetBool(ANIM_IDLE, true);

        if(spine == null) spine = Anim.GetBoneTransform(HumanBodyBones.Spine);
        MouseDownPos = Vector3.zero;
    }



    void Update()
    {   
        if(Anim == null) return; // 애니메이션이 없다면 리턴
        if(GameManager.instance.GameEnd == true) return; // 게임이 끝났다면 리턴

        if(isAim) TryShot();  // 에임 중이면 사격

        if(TryMove()) return;  // 이동 중이면 이후를 생략하고 리턴
        TryIdleAction();    // 대기 모션 체크
    }

    void LateUpdate() 
    {
        TryDetectObj(); // 주변 적 탐지
        Tracking(); // 에임 조준
    }




    // 이동
    bool TryMove()
    {
        // 눌렀을 때
        if(Input.GetMouseButtonDown(0))
        {
            // 애니메이션 변경 부
            Anim.SetInteger(ANIM_RANDOMINT, 0);
            Anim.SetBool(ANIM_IDLE, false);
            Anim.SetBool(ANIM_MOVE, true);

            // 마우스가 눌린 위치와 시간 저장
            MouseDownPos = Input.mousePosition;
            MoveStart = Time.time;
            return true;
        }

        // 누르는 중
        if(Input.GetMouseButton(0))
        {
            var delta = Input.mousePosition - MouseDownPos; // 현재 마우스 거리와 마우스가 눌린 거리 체크 (벡터)
            if(delta.magnitude > CanMove) // 벡터의 길이가 최소 거리를 넘었다면
            {
                var angle = Mathf.Atan2(delta.x, delta.y) * Mathf.Rad2Deg;  // 벡터의 아크탄젠트를 구하고(Atan2) 디그리로 변환(Rag2Deg)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0.0f, angle, 0.0f), 10.0f * Time.deltaTime); // 플레이어를 angle방향을 각도계로 변환하고(Euler) 해당 방향을 자연스럽게 회전하도록 한다
            }

            var moveTime = Time.time - MoveStart;   // 이동시간 측정
            var currentSpeed = Mathf.Lerp(0.0f, Speed, MoveCurve.Evaluate(moveTime / MoveAccelTime)); // 현재 속력을 커브에 비교에 증가하도록한다 >> 현재는 0f 부터 1f까지 이니 Normalization 한 것이라고 생각하면 편하다
            
            Anim.SetFloat(ANIM_MOVE_SPEED, currentSpeed);   // 애니메이션 블렌드 속도 변환
            transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World); // 캐릭터 이동

            return true;
        }

        // 땟을 때
        if(Input.GetMouseButtonUp(0))
        {
            lastIdleActionAt = Time.time;   // 대기 액션 시간 체크
            MoveStop = Time.time;   // 멈춘 시간 체크
            Anim.SetFloat(ANIM_MOVE_SPEED, 0); // 애니메이션 변환           

            // 애니메이션 파라미터 변환
            Anim.SetBool(ANIM_MOVE, false); 
            Anim.SetBool(ANIM_IDLE, true);

            return true;
        }

        return false;
    }

    void Tracking()
    {
        if (targetenemy != null) // 타겟이 있다면
        {
            spine.rotation = Quaternion.LookRotation(targetenemy.transform.position); // 타겟 방향으로 회전
            spine.LookAt(targetenemy.transform.position); // 타겟을 바라봄
            isAim = true; // 에임 상태 체크
        }
    }

    // 탐색
    bool TryDetectObj()
    {
        Collider[] coll = Physics.OverlapSphere(transform.position, Range);  // Range 사거리의 구체를 만들어 구 안의 오브젝트의 콜라이더를 배열에 넣음
        float min = 0f; // 가장 가까이 있는 오브젝트의 거리
        bool istarget = false; // 타겟이 있는지 유무

        if (coll != null)   // 겹치는 오브젝트가 있다면
        {
            foreach (Collider col in coll)
            {
                if (col.tag != "enemy") continue; // 태그가 enemy가 아니면 continue

                if (Physics.Linecast(transform.position, col.transform.position, LayerMask.GetMask("Wall")) == false) // 플레이어와 적 사이에 벽이 없다면
                {
                    float dis = Vector3.Distance(transform.position, col.transform.position); // 목표와의 거리

                    if (min == 0f || dis < min) // 오브젝트가 아직 없거나 지정된 타겟보다 거리가 가깝다면
                    {
                        min = dis;  // 거리 저장
                        targetenemy = col.gameObject; // 타켓 지정
                        istarget = true;
                    }
                }
            }
        }

        else
        {
            targetenemy = null; // 주변에 적이 없다면 타겟을 없앰
            isAim = false; // 에임 상태 체크
        }

        return istarget;
    }


    // 사격
    void TryShot()
    {
        if (targetenemy != null && Time.time - lastShootAt > ShootCoolTime) // 타켓이 있고 쿨타임이 지났다면
        {
            if(CurrentBullet > 0)   // 현재 총알이 남아있다면
            {
                // 애니메이션 파라미터 변경
                Anim.SetInteger(ANIM_RANDOMINT, 0);
                Anim.SetBool(ANIM_IDLE, false);
                Anim.SetBool(ANIM_SHOOT, true);
                
                if(targetenemy != null && targetenemy.TryGetComponent<Enemy>(out Enemy enemycomponent)) // 타켓이 있고 타켓이 Enemy컴포넌트를 가지고 있다면
                {
                    StartCoroutine(ParticleCoroutine(transform.position, targetenemy.transform.position)); // 파티클 처리
                    bool die = enemycomponent.TakeDamage(Random.Range(10,21), this.gameObject); // 데미지 주고 죽었는지 반환
                    camera.GetComponent<PlayerCamera>().Shake(); // 카메라 쉐이크
                    
                    ST_Shoot(); // 샷 처리
                    SoundManager.instance.PlayAudio("ShotSound", ShotSound);
                    CurrentBullet--; // 총알 감소
                    GameManager.instance.UpdateBullet(); // UI 업데이트
                    targetenemy = null; // 타겟 해제

                    if(die) { TryDetectObj(); } // 타겟이 죽었다면 다시 주변을 탐색해서 주변에 적이 있는지 확인
                }
                if (CurrentBullet == 0) // 총이 없으면 자동 장전
                {
                    Anim.SetBool(ANIM_SHOOT, false);
                    Anim.SetBool(ANIM_RELOAD, true);
                    SoundManager.instance.PlayAudio("ReloadSound", ReloadSound, 0.5f);
                }
                
                return;
            }
        }
    }

    // 대기 모션
    bool TryIdleAction()
    {
        if(Time.time - lastIdleActionAt < IdleActionInterval || isAim)  //에임상태거나 일정시간 전이면 리턴
        {
            return false;
        }

        lastIdleActionAt = Time.time; // 마지막 액션 시간 체크
        Anim.SetInteger(ANIM_RANDOMINT, UnityEngine.Random.Range(1, 3)); // 1, 2번 중에 랜덤 실행
        Debug.Log("ActionStart");
        return true;
    }


    // 파티클 코루틴
    IEnumerator ParticleCoroutine(Vector3 _player, Vector3 _enemy)
    {
        var rayzer = Instantiate(Rayzer, spine.position, Quaternion.LookRotation(_enemy - _player));
        rayzer.GetComponent<ParticleSystem>().Play();

        while(Vector3.Distance(rayzer.transform.position, _enemy) > 0.1f)
        {
            rayzer.transform.position = Vector3.Lerp(rayzer.transform.position, _enemy, 0.1f);
            yield return null;
        }

        rayzer.GetComponent<ParticleSystem>().Stop();
    }



    // 애니메이션 이벤트 호출 부 (애니메이션이 끝날 때 애니메이션에서 호출)
    void ST_Action() // 대기 액션
    {
        Anim.SetInteger(ANIM_RANDOMINT, 0);
        lastIdleActionAt = Time.time;
        return;
    }

    void ST_Reload() // 장전
    {
        Anim.SetBool(ANIM_RELOAD, false);
        Anim.SetBool(ANIM_IDLE, true);
        CurrentBullet = MaxBullet;
        lastIdleActionAt = Time.time;
        return;
    }

    void ST_Shoot() // 사격
    {
        Anim.SetBool(ANIM_SHOOT, false);
        Anim.SetBool(ANIM_IDLE, true);
        lastIdleActionAt = Time.time;
        lastShootAt = Time.time;
        return;
    }
}