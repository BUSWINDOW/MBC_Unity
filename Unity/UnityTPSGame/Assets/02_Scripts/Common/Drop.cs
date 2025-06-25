using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        if (this.transform.childCount == 0)
        {
            Drag.draggingItem.transform.SetParent(this.transform, false); // false : 로컬 트랜스폼인지 아니면 월드인지
        }
    }
}
