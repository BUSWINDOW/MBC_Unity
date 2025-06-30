using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class Drop : MonoBehaviour,IDropHandler
{
    //슬롯: 장비 데이터를 얘가 가지고있음
    //그걸 플레이어쪽에 적용
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
