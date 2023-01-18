using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //DetectObj detectObj = new DetectObj();
    
    static readonly int ANIM_IDLE = Animator.StringToHash("isIdle");
    static readonly int ANIM_MOVE = Animator.StringToHash("isMove");
    static readonly int ANIM_SHOOT = Animator.StringToHash("isShoot");
    static readonly int ANIM_RELOAD = Animator.StringToHash("isReload");
    static readonly int ANIM_RANDOMINT = Animator.StringToHash("RandomInt");
    static readonly int ANIM_MOVE_SPEED = Animator.StringToHash("Blend");
    
    [SerializeField] Animator Anim = null;
    //[SerializeField] GameObject Gun = null;


    [Space]
    [SerializeField] float Speed = 7.5f;
    [SerializeField] float CanMove = 10.0f; //최소 이동 거리(_moveThreshold)
    [SerializeField] Vector3 MouseDownPos = Vector3.zero;
    [SerializeField] float MoveStart = 0.0f;
    [SerializeField] float MoveStop = 0.0f;
    [SerializeField] float lastIdleActionAt = 0.0f;
    [SerializeField] float lastShootAt = 0.0f;
    
    [Space]
    [SerializeField] AnimationCurve MoveCurve = null;
    [SerializeField] float MoveAccelTime = 1.0f;
    
    [Space]
    [SerializeField] int MaxBullet = 7;
    [SerializeField] int CurrentBullet = 7;
    [SerializeField] bool isAim = false;
    [SerializeField] GameObject targetenemy = null;
    [Space]
    [SerializeField] float IdleActionInterval = 3.0f;
    [SerializeField] float ShootCoolTime = 1.2f;

    private Transform spine;

    void Awake()
    {
        CurrentBullet =  MaxBullet;
        Anim.SetBool(ANIM_IDLE, true);
        spine = Anim.GetBoneTransform(HumanBodyBones.Spine);
    }

    void Update()
    {       
        //if(Input.GetKeyDown(KeyCode.Space))
        
        TryShot();
        

        if(Anim == null)
            return;

        if(TryMove())
            return;

        TryIdleAction();
    }

    void LateUpdate() 
    {
        TryDetectObj();
    }




    bool TryMove()
    {

        if(Input.GetMouseButtonDown(0))
        {
            Anim.SetInteger(ANIM_RANDOMINT, 0);
            Anim.SetBool(ANIM_IDLE, false);
            Anim.SetBool(ANIM_MOVE, true);

            MouseDownPos = Input.mousePosition;
            MoveStart = Time.time;
            return true;
        }

        if(Input.GetMouseButton(0)) //여기서부터 다시공부
        {
            var delta = Input.mousePosition - MouseDownPos;
            if(delta.magnitude > CanMove)
            {
                var angle = Mathf.Atan2(delta.x, delta.y) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0.0f, angle, 0.0f), 10.0f * Time.deltaTime); // ?
            }

            var moveTime = Time.time - MoveStart;
            var currentSpeed = Mathf.Lerp(0.0f, Speed, MoveCurve.Evaluate(moveTime / MoveAccelTime));
            
            Anim.SetFloat(ANIM_MOVE_SPEED, currentSpeed);
            transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);

            return true;
        }

        if(Input.GetMouseButtonUp(0))
        {
            lastIdleActionAt = Time.time;

            MoveStop = Time.time;
            Anim.SetFloat(ANIM_MOVE_SPEED, 0);            

            Anim.SetBool(ANIM_MOVE, false);
            Anim.SetBool(ANIM_IDLE, true);
        }

        return false;
    }

    //조준 & 사격 부
    bool TryDetectObj()
    {
        Collider[] coll = Physics.OverlapSphere(transform.position, 10.0f);
        float min = 0f;


        foreach (Collider col in coll)
        {
            if(col.tag != "enemy")
                continue;
            
            if(Physics.Linecast(transform.position, col.transform.position, LayerMask.GetMask("Wall")) == false)
            {
                float dis = Vector3.Distance(gameObject.transform.position, col.transform.position); //현재 거리

                if(min == 0f || dis < min)
                {
                    min = dis;
                    targetenemy = col.gameObject;
                }
            }
        }

        if(targetenemy != null)
        {
            spine.rotation = Quaternion.LookRotation(targetenemy.transform.position);
            spine.LookAt(targetenemy.transform.position);
            isAim = true;
            return true;
        }

        isAim = false;
        return false;
    }

    void TryShot()      //리로드 중 사격됨 처리
    {
        if(isAim && Time.time - lastShootAt > ShootCoolTime)
        {
            if(CurrentBullet > 0)
            {
                Anim.SetInteger(ANIM_RANDOMINT, 0);
                Anim.SetBool(ANIM_IDLE, false);
                Anim.SetBool(ANIM_SHOOT, true);
                
                if(targetenemy != null && targetenemy.TryGetComponent<Enemy>(out Enemy enemycomponent))
                {
                    enemycomponent.TakeDamage(Random.Range(10,21), this.gameObject);
                }

                CurrentBullet--;
                //Debug.Log(CurrentBullet);
                lastIdleActionAt = Time.time;
                lastShootAt = Time.time;
                
                if(CurrentBullet == 0)
                {
                    Anim.SetBool(ANIM_SHOOT, false);
                    Anim.SetBool(ANIM_RELOAD, true);
                }
                    return;
            }
        }
    }

    bool TryIdleAction()
    {
        if(Time.time - lastIdleActionAt < IdleActionInterval && !isAim)
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
    Anim.SetInteger(ANIM_RANDOMINT, 0);
    lastIdleActionAt = Time.time;
    return;
}

void ST_Reload()
{
    Anim.SetBool(ANIM_RELOAD, false);
    Anim.SetBool(ANIM_IDLE, true);
    CurrentBullet = MaxBullet;
    lastIdleActionAt = Time.time;
    return;
}

void ST_Shoot()
{
    Anim.SetBool(ANIM_SHOOT, false);
    Anim.SetBool(ANIM_IDLE, true);
    lastIdleActionAt = Time.time;
    return;
}

// void OnTriggerEnter(Collider other) 
// {
//     if(other.gameObject.tag != "Ground")
//     {
//         detectObj.DetectActor(other, this.GetComponent<Transform>().position);
//     }       
// }

// void OnTriggerExit(Collider other) 
// {
//     if(other.gameObject.tag != "Ground")
//     {
//         detectObj.DetectActor(other);
//     }    
// }







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
