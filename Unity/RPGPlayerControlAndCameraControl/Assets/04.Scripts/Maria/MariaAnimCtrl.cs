using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MariaAnimCtrl : MonoBehaviour
{
    private Animator anim;
    private const string isRun = "IsRun";
    private const string isGrounded = "IsGrounded";
    private const string speedX = "SpeedX";
    private const string speedY = "SpeedY";
    private const string swordAttack = "SwordAttack";
    private const string shieldAttack = "ShieldAttack";
    void Start()
    {
        this.anim = GetComponent<Animator>();
    }
    public void SetRun(bool value)
    {
        this.anim.SetBool(isRun, value);
    }
    public void SetGrounded(bool value)
    {
        this.anim.SetBool(isGrounded, value);
    }
    public void SetSpeedX(float value)
    {
        this.anim.SetFloat(speedX, value); // �ִϸ����Ϳ��� X�� �ӵ� ����
    }
    public void SetSpeedY(float value)
    {
        this.anim.SetFloat(speedY, value); // �ִϸ����Ϳ��� X�� �ӵ� ����
    }
    public void SetAttack1()
    {
        this.anim.SetTrigger(swordAttack);
    }
    public void SetAttack2()
    {
        this.anim.SetTrigger(shieldAttack);
    }
}
