using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    static readonly int ANIM_IDLE = Animator.StringToHash("isIdle");
    static readonly int ANIM_MOVE = Animator.StringToHash("isMove");
    static readonly int ANIM_SHOOT = Animator.StringToHash("isShoot");
    static readonly int ANIM_RELOAD = Animator.StringToHash("isReload");
    static readonly int ANIM_RANDOMINT = Animator.StringToHash("RandomInt");
    
    [SerializeField] Animator Anim = null;

    [Space]
    [SerializeField] float Speed = 0.0f;
    [SerializeField] Vector3 mouseDownPos = Vector3.zero;
    //[SerializeField] float MoveStartAt = 0.0f;
    //[SerializeField] float MoveStopAt = 0.0f;
    [SerializeField] float lastIdleActionAt = 0.0f;
    
    [Space]
    [SerializeField] int MaxBullet = 7;
    [SerializeField] int CurrentBullet = 7;

    [Space]
    [SerializeField] float IdleActionInterval = 3.0f;

    void Awake()
    {
        CurrentBullet =  MaxBullet;
        Anim.SetBool(ANIM_IDLE, true);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TryShot();
        }

        if(Anim == null)
            return;

        if(TryMove())
            return;

        TryIdleAction();
    }

    bool TryMove()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Anim.SetInteger(ANIM_RANDOMINT, 0);
            Anim.SetBool(ANIM_IDLE, false);
            Anim.SetBool(ANIM_MOVE, true);
        }

        if(Input.GetMouseButton(0))
        {
            Debug.Log("press");
            lastIdleActionAt = Time.time;
            return true;
        }

        if(Input.GetMouseButtonUp(0))
        {
            Anim.SetBool(ANIM_MOVE, false);
            Anim.SetBool(ANIM_IDLE, true);
        }


        return false;
    }

    void TryShot()      //리로드 중 사격됨 처
    {
        if(CurrentBullet > 0)
        {
            Anim.SetInteger(ANIM_RANDOMINT, 0);
            Anim.SetBool(ANIM_IDLE, false);
            Anim.SetBool(ANIM_SHOOT, true);

            CurrentBullet--;
            Debug.Log(CurrentBullet);
            lastIdleActionAt = Time.time;
            
            if(CurrentBullet == 0)
            {
                Anim.SetBool(ANIM_SHOOT, false);
                Anim.SetBool(ANIM_RELOAD, true);
            }
                return;
        }
    }

    bool TryIdleAction()
    {
        if(Time.time - lastIdleActionAt < IdleActionInterval)
        {
            return false;
        }

        lastIdleActionAt = Time.time;
        Anim.SetInteger(ANIM_RANDOMINT, UnityEngine.Random.Range(1, 3));
        return true;
    }

//애니메이션 이벤트 호출 부

void ST_Action()
{
    Debug.Log("Action");
    Anim.SetInteger(ANIM_RANDOMINT, 0);
    lastIdleActionAt = Time.time;
    return;
}

void ST_Reload()
{
    Debug.Log("Reload");
    Anim.SetBool(ANIM_RELOAD, false);
    Anim.SetBool(ANIM_IDLE, true);
    CurrentBullet = MaxBullet;
    lastIdleActionAt = Time.time;
    return;
}

void ST_Shoot()
{
    Debug.Log("Shoot");
    Anim.SetBool(ANIM_SHOOT, false);
    Anim.SetBool(ANIM_IDLE, true);
    lastIdleActionAt = Time.time;
    return;
}


    // public float speed = 0f;    //이동속도 관리
    // int bullet = 7; //탄창 수
    
    // public Vector3 movePoint;   //이동지점
    // public Camera mainCamera;   //플레이어카메라

    // public bool isMove;     //움직이는 상태
    // public bool isIdle;     //대기상태
    // public bool isShoot;    //발사상태
    // public bool isReload;   //장전상태

    // public float timedelta = 0f;    //애니메이션 시간체크
    // Animator animator;



    // void Start()
    // {
    //     mainCamera = Camera.main;
    //     animator = GetComponent<Animator>();
    //     isIdle = true;
    // }


    // void Update()
    // {
    //     Debug.Log(Time.time);
    //     timedelta += Time.deltaTime;        //애니메이션 시간체크

    //     //Input.GetMouseButtonDown(0) >> 캐릭터 방향 바꾸기

    //     if(Input.GetMouseButton(0))         //왼쪽버튼 눌리면 이동
    //     {
    //         RaycastHit hit;
    //         if(Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit))      //히트 포인트 체크
    //         {
    //             SetDestination(new Vector3(hit.point.x, 0, hit.point.z));       //여기서 임시로 x와 z 값만 가져옴. 네비메시가 없어서 캐릭터가 공중에 뜨는 현상 때문.
    //         }
    //     }
        
        
    //     if(Input.GetMouseButtonUp(0))       //왼클릭 떼면 멈추기
    //     {
    //         Stop();
    //         ResetTimedelta();
    //     }
    //     Move();




    //     if(Input.GetKey(KeyCode.Space) && bullet > 0)   //스페이스바 누르면 상태 바꾸고 발사. 아직 발사기능은 구현 X
    //     {
    //         isShoot = true; isIdle = false;
    //         animator.SetBool("isShoot", isShoot);
    //         animator.SetBool("isIdle", isIdle);
    //         Debug.Log("발사");
    //         bullet--;
    //         if(bullet == 0)
    //         {
    //             StopShoot();
    //             isReload = true; isIdle = false;
    //             animator.SetBool("isReload", isReload);
    //             animator.SetBool("isIdle", isIdle);
    //         }

    //     }

        
    //     if(isIdle && timedelta > 5f)    //대기 5초마다 반복
    //     {
    //         RepeatIdlePattern();
    //     }
            
    // }
    


    // void RepeatIdlePattern()        //애니메이션 반복
    // {
    //     int tmp = Random.Range(1,3);
    //     animator.SetInteger("RandomInt", tmp);
    // }

    // private void SetDestination(Vector3 mov)        //무브포인트 저장
    // {
    //     movePoint = mov;
    //     isMove = true; isIdle = false;
    //     animator.SetBool("isMove", isMove);
    //     animator.SetBool("isIdle", isIdle);
    // }

    // void StopShoot()        //발사 멈추기
    // {
    //     isShoot = false; isIdle = true;
    //     animator.SetBool("isShoot", isShoot);
    //     animator.SetBool("isIdle", isIdle);
    // }

    // void StopReload()   //장전 멈추기
    // {   
    //     bullet = 7;
    //     isReload = false; isIdle = true;
    //     animator.SetBool("isReload", isReload);
    //     animator.SetBool("isIdle", isIdle);
    // }

    // void ResetTimedelta()       //타임리셋
    // {
    //     Debug.Log("호출"); // >> 호출안됨
    //     timedelta = 0f;
    //     animator.SetInteger("RandomInt", 0);
    // }



    // public void Move()
    // {
    //     if(isMove)
    //     {
    //         speed+= 0.005f; //타임델타
    //         if(Vector3.Distance(movePoint, transform.position) <= 0.1f)
    //         {
    //             Stop();
    //         }

    //         var dir = movePoint - transform.position;
    //         transform.forward = dir;
    //         animator.SetFloat("Blend", speed);
    //         transform.position += dir.normalized * Time.deltaTime * Mathf.Clamp(speed, 0f, 5f);
    //     }
    // }
    

    // public void Stop()
    // {
    //     isMove = false; isIdle = true;
    //     animator.SetBool("isMove", isMove);
    //     animator.SetBool("isIdle", isIdle);
    //     speed = 0f;
    //     return;        
    // }

}
