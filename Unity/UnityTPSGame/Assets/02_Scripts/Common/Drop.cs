using System.Collections;
using System.Collections.Generic;
using DataInfo;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        if (this.transform.childCount == 0)
        {
            Drag.draggingItem.transform.SetParent(this.transform, false); // false : 로컬 트랜스폼인지 아니면 월드인지

            //슬롯에 추가된 아이템을 GameData에 추가
            Item item = Drag.draggingItem.GetComponent<ItemInfo>().itemData; 
            GameManager.Instance.AddItem(item);
        }
    }
}
