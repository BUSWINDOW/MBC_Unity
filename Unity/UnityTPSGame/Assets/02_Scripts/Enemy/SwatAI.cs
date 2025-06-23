using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyAI;
[RequireComponent(typeof(Animator))]

public class SwatAI : MonoBehaviour
{
    public eState state = eState.Patrol;

    private Transform playerTr;
    private MoveAgent moveAgent;
    private SwatFire e_Fire;

    private SwatAnimationCtrl animCtrl;

    //공격 사정거리
    public float attackDist = 5.0f; //공격 범위 총알 발사 사정 거리
    public float traceDist = 10f; // 추적 시작 범위

    public bool isDie = false;
    private WaitForSeconds ws;



    void Awake()
    {
        this.playerTr = GameObject.FindWithTag("Player").transform;
        this.e_Fire = GetComponent<SwatFire>();
        this.ws = new WaitForSeconds(0.3f);
        this.moveAgent = GetComponent<MoveAgent>();
        this.animCtrl = GetComponent<SwatAnimationCtrl>();
    }
    private void OnEnable()
    {
        StartCoroutine(this.CheckState());
        StartCoroutine(CheckAction());

    }
    IEnumerator CheckState()
    {
        while (!isDie)
        {
            //Debug.Log(this.state);
            if (state == eState.Die) yield break;

            float dist = Vector3.Distance(this.transform.position, playerTr.position);
            if (dist <= attackDist)
            {
                this.state = eState.Attack;
            }
            else if (dist <= traceDist)
            {
                this.state = eState.Trace;
            }
            else
            {
                this.state = eState.Patrol;
            }

            yield return ws;
        }

    }
    IEnumerator CheckAction()
    {
        while (!isDie)
        {
            yield return this.ws;
            switch (this.state)
            {
                case eState.Patrol:
                    {
                        this.moveAgent.Patrolling = true;
                        this.animCtrl.SetPatroll(this.moveAgent.Speed);
                        this.e_Fire.isFire = false;

                        break;
                    }
                case eState.Trace:
                    {
                        this.moveAgent.TraceTarget = this.playerTr.position;
                        this.animCtrl.SetTrace(this.moveAgent.Speed);
                        this.e_Fire.isFire = false;



                        break;
                    }
                case eState.Attack:
                    {
                        this.moveAgent.Stop();
                        this.animCtrl.SetAttack();
                        this.e_Fire.isFire = true;
                        break;
                    }
                case eState.Die:
                    {
                        //Die();

                        this.moveAgent.Stop();
                        break;
                    }

            }
        }
    }

    /*private void Die()
    {
        //Debug.Log("Die");
        this.isDie = true;
        this.e_Fire.isFire = false;
        this.e_Fire.isReload = false;
        this.moveAgent.agent.isStopped = true;
        this.animCtrl.SetDie();
        this.GetComponent<CapsuleCollider>().enabled = false;
    }*/
}
