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
        
        if(screenPos.z < 0) // ī�޶��� ���� ����(180��) ȸ���϶� ��ǥ���� ���ļ� ����
        {
            screenPos.z *= -1;
        }

        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectParent // �θ� Ʈ������
            , screenPos, //��ũ�� ��ǥ�� ��ȯ�� ��
            uiCamera,  // ui�� �� ī�޶�
            out localPos); // ������ǥ�� ��ȯ�� ��
        rectHp.localPosition = localPos;
    }
}
