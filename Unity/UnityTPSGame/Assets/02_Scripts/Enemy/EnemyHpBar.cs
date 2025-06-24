using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    private Camera uiCamera;
    private Canvas uiCanvas;
    private RectTransform rectParent;
    private RectTransform rectHp;
    [HideInInspector] public Vector3 offset = Vector3.zero;
    [HideInInspector] public Transform targetTr;
    void Start()
    {
        uiCanvas = GetComponentInParent<Canvas>();
        uiCamera = uiCanvas.worldCamera;
        rectParent = uiCanvas.GetComponent<RectTransform>();
        rectHp = this.GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        var screenPos = Camera.main.WorldToScreenPoint(this.targetTr.position + offset);
        
        if(screenPos.z < 0) // 카메라의 뒷쪽 영역(180도) 회전일때 좌표값을 고쳐서 보정
        {
            screenPos.z *= -1;
        }

        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectParent // 부모 트랜스폼
            , screenPos, //스크린 좌표로 변환한 값
            uiCamera,  // ui가 뜰 카메라
            out localPos); // 로컬좌표로 변환한 값
        rectHp.localPosition = localPos;
    }
}
