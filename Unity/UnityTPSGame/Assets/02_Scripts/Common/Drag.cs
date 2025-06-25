using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Drag : MonoBehaviour,IDragHandler , IBeginDragHandler, IEndDragHandler
{
    [SerializeField]private Transform inventoryTr;

    public static GameObject draggingItem = null;

    [SerializeField]private Transform itemListTr;
    private CanvasGroup cvGroup;

    void Start()
    {
        //this.inventoryTr = GetComponentInParent<Transform>(); //이렇게 쓰면 안된다, 아이템 이동 몇번 시키다보면 어디가 부모일지 꼬이게 된다.
        this.cvGroup = this.GetComponent<CanvasGroup>();
    }

    //드래그 이벤트
    public void OnDrag(PointerEventData eventData) // 드래그 중
    {
        this.transform.position = Input.mousePosition;
        
    }

    public void OnBeginDrag(PointerEventData eventData) //드래그 시작
    {
        this.transform.SetParent(this.inventoryTr);
        draggingItem = this.gameObject; // 드래그가 시작되면 드래그 되는 아이템 정보를 저장함
        this.cvGroup.blocksRaycasts = false; // 드래그가 시작되면 다른 UI 이벤트를 받지 않도록 설정
    }

    public void OnEndDrag(PointerEventData eventData) //드래그 끝
    {
        draggingItem = null;
        this.cvGroup.blocksRaycasts = true; // 드래그가 끝나면 다시 UI 이벤트를 받게 설정

        //슬롯에 드래그 되지 않았을 경우, 원래대로 돌아온다.
        if(this.transform.parent == this.inventoryTr)
        {
            this.transform.SetParent(this.itemListTr.transform);
        }
    }
}
