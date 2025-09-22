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
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    private readonly int hashReset = Animator.StringToHash("Reset");
    private bool isDie = false;

    public Action dieAction;

    public Canvas hpUI;

    void Start()
    {
        this.agent = GetComponent<NavMeshAgent>();
        this.playerTr = GameObject.FindWithTag("Player").transform;
        this.hpUI = GetComponentInChildren<Canvas>();
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
                Die();
                //this.dieAction();
            }
            else
            {
                this.animator.SetTrigger(hashHit);
            }
        };

        
        Player.dieAction += () =>
        {
            this.animator.SetTrigger(hashPlayerDie);
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        };
        this.hp = this.maxHP;
    }

    private void Die()
    {
        this.isDie = true;
        this.GetComponent<CapsuleCollider>().enabled = false;
        this.animator.SetTrigger(hashDie);
        this.hpUI.gameObject.SetActive(false);
        StartCoroutine(this.DieRoutine());
    }
    IEnumerator DieRoutine()
    {
        yield return new WaitForSeconds(3);
        this.gameObject.SetActive(false);
        this.animator.SetTrigger(hashReset);
        this.hpUI.gameObject.SetActive(true);
        this.hp = 100;
        this.hp_Bar.fillAmount = (float)hp / (float)maxHP;
        this.hp_Text.text = $"HP : <color=#ff0000>{hp}</color>";
        this.hp_Bar.color = new Color(1 - this.hp_Bar.fillAmount, this.hp_Bar.fillAmount, 0);
        
        this.isDie = false;
        this.GetComponent<CapsuleCollider>().enabled = true;
        
    }
    private void OnEnable()
    {
        BarrelCtrl.OnExplodAction += Die;
    }
    private void OnDisable()
    {
        BarrelCtrl.OnExplodAction -= Die;
    }

    void Update()
    {
        if (isDie||GameManager.instance.isGameOver)
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
