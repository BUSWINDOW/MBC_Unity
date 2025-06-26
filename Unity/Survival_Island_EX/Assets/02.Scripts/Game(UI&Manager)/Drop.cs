using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class Drop : MonoBehaviour,IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Drag.draggingItem.transform.SetParent(this.transform, false);
        ItemData item = Drag.draggingItem.GetComponent<ItemInfo>().data;
        DataManager.instance.AddItem(item);
    }

}
