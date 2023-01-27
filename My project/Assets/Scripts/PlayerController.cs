using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


// To do .. : 이 클래스에서 UI의 이미지만 처리하고 실질적인 움직임은 캐릭터에서 처리하는 것을 동일화 할 것
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