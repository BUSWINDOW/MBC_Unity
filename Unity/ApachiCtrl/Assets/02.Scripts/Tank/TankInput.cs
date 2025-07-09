using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ws 앞뒤
//ad 좌우 이동
//마우스 휠 포신이 올라가거나 내려감
// 마우스를 돌리면 포신이 돌아감
public class TankInput : MonoBehaviour
{
    //이동 및 회전을 위한 axes 이름
    public readonly string Horizontal = "Horizontal";
    public readonly string Vertical = "Vertical";
    // 마우스 휠 입력을 위한 axes 이름
    public readonly string MouseScrollWheel = "Mouse ScrollWheel";
    // 마우스 회전 입력을 위한 axes 이름
    public readonly string MouseRotation = "Mouse X";
    // 공격 입력을 위한 axes 이름
    public readonly string Fire = "Fire1";



    public float h = 0;
    public float v = 0;
    public float m_scroll = 0;
    public float m_rotation = 0;
    public bool isFire = false;
    void Start()
    {
        
    }

    void Update()
    {
        if(GameManager.Instance == null)
        {
            return;
        }

        if (GameManager.Instance.isGameOver)
        {
            this.h = 0;
            this.v = 0;
            return;
        }
        this.h = Input.GetAxis(Horizontal);
        this.v = Input.GetAxis(Vertical);
        this.m_scroll = Input.GetAxisRaw(MouseScrollWheel); // 마우스 휠 입력
        this.m_rotation = Input.GetAxisRaw(MouseRotation); // 마우스 회전 입력
        this.isFire = Input.GetButtonDown(Fire); // 공격 입력
    }
}
