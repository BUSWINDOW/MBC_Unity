using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ws �յ�
//ad �¿� �̵�
//���콺 �� ������ �ö󰡰ų� ������
// ���콺�� ������ ������ ���ư�
public class TankInput : MonoBehaviour
{
    //�̵� �� ȸ���� ���� axes �̸�
    public readonly string Horizontal = "Horizontal";
    public readonly string Vertical = "Vertical";
    // ���콺 �� �Է��� ���� axes �̸�
    public readonly string MouseScrollWheel = "Mouse ScrollWheel";
    // ���콺 ȸ�� �Է��� ���� axes �̸�
    public readonly string MouseRotation = "Mouse X";
    // ���� �Է��� ���� axes �̸�
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
        this.m_scroll = Input.GetAxisRaw(MouseScrollWheel); // ���콺 �� �Է�
        this.m_rotation = Input.GetAxisRaw(MouseRotation); // ���콺 ȸ�� �Է�
        this.isFire = Input.GetButtonDown(Fire); // ���� �Է�
    }
}
