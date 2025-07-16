using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class WomanInput : MonoBehaviourPun
{
    // Input값 관리

    public const string rotateAxisName = "Horizontal"; //회전값 입력
    public const string moveAxisName = "Vertical"; // 이동값 입력
    public const string fireButtonName = "Fire1"; // 공격버튼 입력
    public const string reloadButtonName = "Reload"; //재장전 버튼 입력

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
        if (!photonView.IsMine) return; // 로컬이 아니라면 입력 X
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
