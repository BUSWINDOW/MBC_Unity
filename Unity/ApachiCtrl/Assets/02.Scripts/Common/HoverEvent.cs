using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static HoverEvent instance;

    public bool isDown = false;
    public bool isEnter = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        this.isDown = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.isEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.isEnter = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        this.isDown = false;
    }

    void Start()
    {
        instance = this;
    }
}
