using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{

    public static GameObject draggingItem = null;
    public GameObject inventory;
    public GameObject itemSlot;
    CanvasGroup cvGroup;

    void Start()
    {
        this.cvGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        draggingItem = this.gameObject;
        this.transform.SetParent(this.inventory.transform);
        this.cvGroup.blocksRaycasts = false; // 드래그가 시작되면 다른 UI 이벤트를 받지 않도록 설정
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draggingItem = null;
        this.cvGroup.blocksRaycasts = true;
        if (this.transform.parent == this.inventory.transform)
        {
            this.transform.SetParent(this.itemSlot.transform);
        }
    }


}
