using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private readonly string speed = "Speed";
    private readonly int attackHash = Animator.StringToHash("ComboAttack");
    private readonly int skillHash = Animator.StringToHash("SkillTrigger");
    private readonly int dashHash = Animator.StringToHash("DashTrigger");


    [SerializeField] Animator animator;
    [SerializeField] Rigidbody rb;
    [SerializeField] CapsuleCollider col;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip swordClip;


    float lastSkillTime, lastDashTime, lastAttackTime;
    bool isAttacking = false;
    bool isDashing = false;
    bool isSkilling = false;
    void Start()
    {
        this.animator = GetComponent<Animator>();
        this.rb = GetComponent<Rigidbody>();
        this.col = GetComponent<CapsuleCollider>();
        this.source = GetComponent<AudioSource>();
        this.swordClip = Resources.Load<AudioClip>("Sounds/coconut_throw");
    }
    float h = 0, v = 0;
    public void OnStickPos(Vector3 stickPos)
    {
        this.h = stickPos.x;
        this.v = stickPos.y;
    }
    public void OnAttackDown()
    {
        this.isAttacking = true;
        animator.SetBool(attackHash, isAttacking);
        //StartCoroutine(this.ComboAttackTimming());
    }
    IEnumerator ComboAttackTimming()
    {
        if (Time.time - lastAttackTime > 1f)
        {
            lastAttackTime = Time.time;
            while (isAttacking) 
            {
                animator.SetBool(attackHash, isAttacking);
                yield return new WaitForSeconds(1f);
            }
        }
    }
    public void OnAttackUp()
    {
        this.isAttacking = false;
        animator.SetBool(attackHash, isAttacking);
    }
    public void OnDashDown()
    {
        this.isDashing = true;
        this.animator.SetTrigger(dashHash);
    }
    public void OnDashUp()
    {
        this.isDashing = false;
    }
    public void OnSkillDown()
    {
        this.isSkilling = true;
        this.animator.SetTrigger(skillHash);
    }
    public void OnSkillUp()
    {
        this.isSkilling = false;
    }
    public void SkillSound()
    {
        this.source.PlayOneShot(this.swordClip);
    }
    void FixedUpdate()
    {
        if (this.animator != null) 
        {
            animator.SetFloat(speed, (h*h + v*v));
            if (this.rb != null)
            {
                Vector3 speed = rb.velocity;
                speed.x = 4 * h;
                speed.z = 4 * v;
                rb.velocity = speed;
                if(MathF.Abs(h) > 0.001f && MathF.Abs(v) > 0.001f)
                {
                    
                    //Debug.Log($"{h} , {v}");
                    this.transform.rotation = Quaternion.LookRotation(new Vector3(h, 0, v));
                }
            }

        }
    }
}
