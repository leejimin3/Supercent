using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeTemplate : MonoBehaviour
{
}

/*

카메라

 카메라 비율 조정
 ==================================================================

    void Awake() //핸드폰의 크기에 따라 9:16비율로 설정
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)9 / 16);
        float scalewidth = 1f / scaleheight;
        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        camera.rect = rect;
    }
}
==================================================================

카메라 흔들림 효과
==================================================================
    public void Shake()
    {
        StartCoroutine(CameraShake(0.1f, 0.1f)); //0.1초 동안 0.1의 범위로
    }

    IEnumerator CameraShake(float time, float mag)
    {
        float timer = 0.0f;
        Vector3 pos = transform.position;
        while(timer <= time)
        {
            transform.localPosition = Random.insideUnitSphere * mag + pos;

            timer += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = pos;
    }
==================================================================

카메라 트래킹 >> Update를 통한 프레임 단위 추적이니 카메라의 위치를 바꿀 때 조심할 것
==================================================================

    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    void LateUpdate() // >> LateUpdate
    {
        transform.position = player.transform.position + offset;
    }

==================================================================









플레이어

Animator 파라미터 받아오기 & SerializeField
==================================================================

    static readonly int ANIM_IDLE = Animator.StringToHash("isIdle"); // 해시로 바꿔 멤버변수로 선언 >> 어떤 형이든 Hash로 변형되기 때문에 int형으로 받음
    Anim.SetBool(ANIM_IDLE, true); // >> 멤버 변수로 선언 후 이런 식으로 Get, Set가능.


    [Space] //인스펙터 창에 공간
    [Header("Camera")] // 인스펙터 창에 소제목
    [SerializeField] Camera camera = null;
==================================================================

캐릭터 이동 (터치 후 드래그 방향으로)
==================================================================
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
            var currentSpeed = Mathf.Lerp(0.0f, Speed, MoveCurve.Evaluate(moveTime / MoveAccelTime)); // 현재 속력을 커브의 각도와 비교해 Normalize한다.
            
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

    //플레이어에 선언해주고 아래의 방법으로 UI처리

    public class PlayerController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private RectTransform lever;
        [SerializeField] private RectTransform rectTransform;

        [SerializeField, Range(0.01f, 150)]
        private float leverRange = 100.0f;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            var inputPos = eventData.position - rectTransform.anchoredPosition;
            rectTransform.anchoredPosition = new Vector2(eventData.position.x, eventData.position.y);

            //rectTransform.position = eventData.position;
            var InputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
            lever.anchoredPosition = InputVector;
        }

        public void OnDrag(PointerEventData eventData)
        {
            var inputPos = eventData.position - rectTransform.anchoredPosition;
            var InputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
            lever.anchoredPosition = InputVector;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            lever.anchoredPosition = Vector3.zero;
        }

    }

==================================================================

래그돌
==================================================================

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

==================================================================

HP처리 >> 체력이 감소한 후 뒤의 반투명 체력이 따라오는 방법
==================================================================

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

==================================================================

Particle
==================================================================

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

==================================================================

Navi
==================================================================

    void Update()
    {
        if(Isdie)
        {
            navMeshAgent.destination = transform.position;
        }
        if(!Isdie)
        {
            if(Vector3.Distance(transform.position, RandPos) <= 0.1f)
            {
                if(RandomPoint(transform.position, out RandPos))
                {
                    navMeshAgent.destination = RandPos;
                }
            }
        }
    }


    bool RandomPoint(Vector3 center, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * RandomMoveRange;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, 1)) // 1은 NavLayer, 여기서 1은 Walkable Layer
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

==================================================================















UI
==================================================================

    IEnumerator FadeinCoroutine()
    {
        float curfade = 0;
        float panelfade = 0.66f;
        float buttonfade = 1.0f;
        float comtextfade = 1.0f;
        GameObject Panel = CompleteCanvas.GetComponent<Transform>().Find("Panel").gameObject;
        GameObject Button = CompleteCanvas.GetComponent<Transform>().Find("Button").gameObject;
        GameObject ComText = CompleteCanvas.GetComponent<Transform>().Find("CompleteText").gameObject;

        while (curfade < 1.0f)
        {

            curfade += 0.01f;
            if(curfade < panelfade)
                Panel.GetComponent<Image>().color = new Color(0, 0, 0, curfade);

            if(curfade < buttonfade)
                Button.GetComponent<Image>().color = new Color(0, 0, 0, curfade);

            if (curfade < comtextfade)
                ComText.GetComponent<Text>().color = new Color(0, 0, 0, curfade);

            yield return new WaitForSeconds(0.01f);
        }
    }

==================================================================

*/