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
        //this.inventoryTr = GetComponentInParent<Transform>(); //�̷��� ���� �ȵȴ�, ������ �̵� ��� ��Ű�ٺ��� ��� �θ����� ���̰� �ȴ�.
        this.cvGroup = this.GetComponent<CanvasGroup>();
    }

    //�巡�� �̺�Ʈ
    public void OnDrag(PointerEventData eventData) // �巡�� ��
    {
        this.transform.position = Input.mousePosition;
        
    }

    public void OnBeginDrag(PointerEventData eventData) //�巡�� ����
    {
        this.transform.SetParent(this.inventoryTr);
        draggingItem = this.gameObject; // �巡�װ� ���۵Ǹ� �巡�� �Ǵ� ������ ������ ������
        this.cvGroup.blocksRaycasts = false; // �巡�װ� ���۵Ǹ� �ٸ� UI �̺�Ʈ�� ���� �ʵ��� ����
    }

    public void OnEndDrag(PointerEventData eventData) //�巡�� ��
    {
        draggingItem = null;
        this.cvGroup.blocksRaycasts = true; // �巡�װ� ������ �ٽ� UI �̺�Ʈ�� �ް� ����

        //���Կ� �巡�� ���� �ʾ��� ���, ������� ���ƿ´�.
        if(this.transform.parent == this.inventoryTr)
        {
            this.transform.SetParent(this.itemListTr.transform);
        }
    }
}
