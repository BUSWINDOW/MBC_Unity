using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class _Test_Mover : MonoBehaviour,_Test_IAction
{
    //agent를 이용해서 좌표를 입력받으면 해당 좌표로 agnet의 목적지를 셋팅하는 함수
    NavMeshAgent agent;
    void Start()
    {
        this.agent = GetComponent<NavMeshAgent>();
    }
    public void MoveSet(Vector3 pos)
    {
        this.agent.SetDestination(pos);
    }
    public float GetDistance()
    {
        return this.agent.remainingDistance;
    }
    public void Cancel()
    {
        //이동 취소시 현재 자신의 위치를 목적지로
        this.agent.SetDestination(this.transform.position);
    }
}
