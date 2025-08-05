using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

public class CapDrag : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    private Vector3 startPos = Vector3.zero;

    public Action<float> OnDragAction;
    public Action EndDragAction;
    public Action NotEndDragAction; // 드래그로 충분히 올리지 않았을때의 액션
    public Action DragCompleteAction; // 드래그가 끝났을때의 액션
    private void Start()
    {
        this.startPos = this.transform.position;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = new Vector3(
        this.transform.position.x,
        Input.mousePosition.y,
        0f
    );
        if (this.transform.position.y < this.startPos.y)
        {
            this.transform.position = this.startPos; // 원래 위치보다 내려가려한다면 다시 원래위치로 바꾸는 식으로
                                                     // 위로만 갈수 있게 한다.
        }
        this.OnDragAction(this.transform.position.y - this.startPos.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (this.transform.position.y - this.startPos.y > 250f) // 만약 드래그가 위로 올라갔다면(조건의 임계점은 바꿀 수 있음)
        {
            // 캡을 완전히 위로 올린다(서서히 올라가게)
            this.EndDragAction(); // 액션 쪽에서 알파값을 마저 조절시킨다.
            this.transform.DOLocalMoveY(500f, 1000).SetSpeedBased().OnComplete(() =>
            {
                this.DragCompleteAction();
            });
        }
        else
        {
            this.transform.DOMoveY(this.startPos.y, 1000).SetSpeedBased(); // 원래 위치로 돌아간다.
            this.NotEndDragAction();
        }

    }
}
