using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]

public class MoveAgent : MonoBehaviour
{
    public Transform wayGroup;
    public List<Transform> wayPointList = new List<Transform>();

    public int nextIdx = 0;
    public NavMeshAgent agent;


    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4;
    private bool _patrolling;
    public bool Patrolling 
    {
        get 
        { 
            return _patrolling; 
        }
        set 
        {
            this._patrolling = value;
            if (this._patrolling)
            {
                this.agent.speed = patrolSpeed;
            }
            
        } 
    }

    private Vector3 _traceTarget;
    public Vector3 TraceTarget
    {
        get
        {
            return _traceTarget;
        }
        set
        {
            this._traceTarget = value;
            this.agent.speed = traceSpeed;
            this.TraceTargetAct(this._traceTarget);
        }
    }
    public float Speed { get { return  agent.speed/*agent.velocity.magnitude*/;} }

    void TraceTargetAct(Vector3 pos)
    {
        if (this.agent.isPathStale) return;
        this.agent.destination = pos;
        this.agent.isStopped = false;
    }
    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        this._patrolling = false;
    }

    void Start()
    {
        #region 웨이포인트 잡기 방법 1
        /*Transform[] wayPoints = this.wayGroup.gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform t in wayPoints) 
        {
            this.wayPointList.Add(t);
        }
        this.wayPointList.RemoveAt(0);*/
        #endregion
        #region 웨이포인트 잡기 방법 2
        if (this.wayGroup != null)
        {
            this.wayGroup.GetComponentsInChildren<Transform>(wayPointList);
            wayPointList.RemoveAt(0);
        }
        #endregion
        this.agent = GetComponent<NavMeshAgent>();
        this.agent.autoBraking = false;
        this.MoveWayPoint();
        agent.speed = this.patrolSpeed;
    }
    void MoveWayPoint()
    {
        if (this.agent.isPathStale) //최단 경로 계산이 끝나지 않으면 다음 수행 하지 않는다.
            //stale 신선하지 않은 -> 지금 길이 너무 오래됐다(유효하지 않다)
        {
            return;
        }

        agent.destination = wayPointList[nextIdx++].position;
        this.agent.isStopped = false; // 추적 활성화
        if(nextIdx == this.wayPointList.Count)
        {
            this.nextIdx = 0;
        }
    }
    void Update()
    {
        if (!_patrolling)
        {
            return;
        }
        if(this.agent.remainingDistance < 0.1f)
        {
            MoveWayPoint();
        }
    }
}
