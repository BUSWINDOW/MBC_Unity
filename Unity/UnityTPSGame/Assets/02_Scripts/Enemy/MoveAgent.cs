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

    private float damping = 1.0f; // ī�޶� �ε巴�� �����ϴ� �ɼ�

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
                this.damping = 1; // �����Ҷ� damping�� 1
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
            this.damping = 7;
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

        this.wayGroup = GameObject.Find("WayPointGroup").transform;
        #region ��������Ʈ ��� ��� 1
        /*Transform[] wayPoints = this.wayGroup.gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform t in wayPoints) 
        {
            this.wayPointList.Add(t);
        }
        this.wayPointList.RemoveAt(0);*/
        #endregion
        #region ��������Ʈ ��� ��� 2
        if (this.wayGroup != null)
        {
            this.wayGroup.GetComponentsInChildren<Transform>(wayPointList);
            wayPointList.RemoveAt(0);
            this.nextIdx = Random.Range(0, wayPointList.Count);
        }
        #endregion

        

        this.agent = GetComponent<NavMeshAgent>();
        this.agent.autoBraking = false;
        this.MoveWayPoint();
        agent.speed = this.patrolSpeed;
        this._patrolling = true;
        this.agent.updateRotation = false; // agent�� ��üȸ���� ��(���ڿ� ��������)
    }
    void MoveWayPoint()
    {
        if (this.agent.isPathStale) //�ִ� ��� ����� ������ ������ ���� ���� ���� �ʴ´�.
            //stale �ż����� ���� -> ���� ���� �ʹ� �����ƴ�(��ȿ���� �ʴ�)
        {
            return;
        }
        this.nextIdx = Random.Range(0, this.wayPointList.Count);
        agent.destination = wayPointList[nextIdx/*++*/].position;
        this.agent.isStopped = false; // ���� Ȱ��ȭ
        /*if(nextIdx == this.wayPointList.Count)
        {
            this.nextIdx = 0;
        }*/
    }
    void Update()
    {
        if (!this.agent.isStopped && agent.desiredVelocity != Vector3.zero) //agent�� �����̰� ������
        {
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity); // agentDesiredVelocity(agent�� ���� ����� �ӵ�)
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rot, Time.deltaTime * damping);
        }
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
