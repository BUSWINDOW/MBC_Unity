using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class Drop : MonoBehaviour,IDropHandler
{
    //����: ��� �����͸� �갡 ����������
    //�װ� �÷��̾��ʿ� ����
    bool isEquiped = false;
    public ItemData item;
    public int idx;

    public Action<int> itemEquipAction;
    public Action<int> itemRemoveAction;
    //public Player player;
    public void OnDrop(PointerEventData eventData)
    {
        if (isEquiped)
        {
            return;
        }
        Drag.draggingItem.transform.SetParent(this.transform, false);
        this.item = Drag.draggingItem.GetComponent<ItemInfo>().data;

        this.itemEquipAction(this.idx);
        //
        //DataManager.instance.AddItem(item);
    }
    private void OnTransformChildrenChanged()
    {
        if (!isEquiped)
        {
            isEquiped = true;
            
        }
        else
        {
            isEquiped = false;
            this.itemRemoveAction(this.idx);
            //this.item.Idx = ItemData.eItemIdx.None;
        }
    }
}
