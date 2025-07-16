using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class WomanInput : MonoBehaviourPun
{
    // Input�� ����

    public const string rotateAxisName = "Horizontal"; //ȸ���� �Է�
    public const string moveAxisName = "Vertical"; // �̵��� �Է�
    public const string fireButtonName = "Fire1"; // ���ݹ�ư �Է�
    public const string reloadButtonName = "Reload"; //������ ��ư �Է�

    public float Move
    {
        get; private set;
    }
    public float Rotate
    {
        get; private set;
    }
    public bool Fire
    {
        get; private set;
    }
    public bool Reload
    {
        get; private set;
    }


    void Update()
    {
        if (!photonView.IsMine) return; // ������ �ƴ϶�� �Է� X
        if(GameManager.Instance != null && GameManager.Instance.IsGameOver)
        {
            this.Move = 0;
            this.Rotate = 0;
            this.Fire = false;
            this.Reload = false;
            return;
        }
        this.Move = Input.GetAxis(moveAxisName);
        this.Rotate = Input.GetAxis(rotateAxisName);
        this.Fire = Input.GetButton(fireButtonName);
        this.Reload = Input.GetButtonDown(reloadButtonName);
    }
}
