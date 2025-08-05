using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimCtrl : MonoBehaviour
{
    Animator anim;


    void Start()
    {
        this.anim = GetComponent<Animator>();
    }
    public void RunAnimSet(float speed) 
    {
        this.anim.SetFloat("forwardSpeed", speed);
    }
    public void AttackAnimSet()
    {
        this.anim.SetTrigger("Attack");
    }
}
