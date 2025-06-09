using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
[RequireComponent(typeof(NavMeshAgent))]

public class EnemyCtrl : MonoBehaviour
{
    //맞기, 공격하기 죽기, 이동하기
    private int maxHP = 100;
    public int hp;
    public float chaseDist = 20;
    public float attackDist = 3;
    private NavMeshAgent agent;
    private Transform playerTr;
    private Animator animator;
    private EnemyDamage E_Damage;

    private Image hp_Bar;
    private Text hp_Text;

    private BoxCollider attackCol;

    private readonly int hashTrace = Animator.StringToHash("IsTrace");
    private readonly int hashAttack = Animator.StringToHash("IsAttack");
    private readonly int hashDie = Animator.StringToHash("isDie");
    private readonly int hashHit = Animator.StringToHash("isHit");
    private bool isDie = false;

    public Action dieAction;
    void Start()
    {
        this.agent = GetComponent<NavMeshAgent>();
        this.playerTr = GameObject.FindWithTag("Player").transform;
        this.animator = GetComponent<Animator>();
        this.E_Damage = GetComponent<EnemyDamage>();
        this.attackCol = GetComponentInChildren<BoxCollider>();
        this.hp_Bar = GetComponentsInChildren<Image>()[1];
        this.hp_Text = GetComponentInChildren<Text>();
        this.hp_Bar.color = Color.green;

        this.E_Damage.hitAction += () => 
        {
            this.hp -= 10;
            this.hp_Bar.fillAmount = (float)hp / (float)maxHP;
            this.hp_Text.text = $"HP : <color=#ff0000>{hp}</color>";
            this.hp_Bar.color = new Color(1 - this.hp_Bar.fillAmount, this.hp_Bar.fillAmount, 0);


            if (this.hp <= 0)
            {
                this.isDie = true;
                this.GetComponent<CapsuleCollider>().enabled = false;
                this.animator.SetTrigger(hashDie);
                GetComponentInChildren<Canvas>().gameObject.SetActive(false);
                //this.dieAction();
            }
            else
            {
                this.animator.SetTrigger(hashHit);
            }
        };

        
        this.hp = this.maxHP;
    }

    void Update()
    {
        if (isDie)
        {
            return;
        }
        float dist =  Vector3.Distance(this.transform.position, this.playerTr.position);
        if(dist < attackDist)
        {
            this.animator.SetBool(hashAttack, true);
            this.agent.isStopped = true;
        }
        else if (dist < chaseDist)
        {
            this.agent.SetDestination(this.playerTr.position);
            this.animator.SetBool(hashTrace, true);
            this.agent.isStopped = false;
            this.animator.SetBool(hashAttack, false);
        }
        else
        {
            this.agent.isStopped = false;
            this.animator.SetBool(hashTrace, false);
        }
        
        
    }
    public void SetAttackCol()
    {
        attackCol.enabled = true;
    }
    public void DisableAttackCol()
    {
        attackCol.enabled = false;
    }
}
