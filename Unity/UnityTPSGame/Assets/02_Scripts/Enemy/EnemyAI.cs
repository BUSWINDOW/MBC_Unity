using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
//상태에 따른 공격 패트롤 애니메이션 등을 구현
public class EnemyAI : MonoBehaviour
{
    public enum eState
    {
        Patrol, Trace, Attack, Die
    }
    public eState state = eState.Patrol;

    private Transform playerTr;
    private MoveAgent moveAgent;
    private Animator animator;
    //공격 사정거리
    public float attackDist = 5.0f; //공격 범위 총알 발사 사정 거리
    public float traceDist = 10f; // 추적 시작 범위

    public bool isDie = false;
    private WaitForSeconds ws;
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("Speed");

    void Awake()
    {
        this.animator = GetComponent<Animator>();
        this.playerTr = GameObject.FindWithTag("Player").transform;
        this.ws = new WaitForSeconds(0.3f);
        this.moveAgent = GetComponent<MoveAgent>();
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
            if(state == eState.Die) yield break;

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
                        this.animator.SetBool(hashMove, true);
                        this.animator.SetFloat(hashSpeed, this.moveAgent.Speed);
                        break;
                    }
                case eState.Trace:
                    {
                        this.moveAgent.TraceTarget = this.playerTr.position;
                        this.animator.SetBool(hashMove, true);
                        this.animator.SetFloat(hashSpeed, this.moveAgent.Speed);


                        break;
                    }
                case eState.Attack:
                    {
                        this.moveAgent.Stop();
                        this.animator.SetBool(hashMove, false);
                        break;
                    }
                case eState.Die:
                    {
                        this.moveAgent.Stop();
                        break;
                    }

            }
        }
    }
    private void OnDisable()
    {
        
    }
    void Update()
    {
    }
}
