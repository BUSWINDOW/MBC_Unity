using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkPatrol : MonoBehaviour
{
    [SerializeField]
    Transform[] patrolPoints;
    [SerializeField]
    Transform patrolPointParent;
    [SerializeField]
    Transform target;
    [SerializeField]
    Terrain terrain;
    [SerializeField]
    GridManager gridManager;
    int idx = 0;

    private float speed = 50f;
    private float searchDist = 3000f;
    private float chaseDist = 3000f;
    private float attackDist = 10f;

    public bool test;
    private void Awake()
    {
        this.target = GameObject.FindGameObjectWithTag("Player").transform;
        this.terrain = GameObject.FindFirstObjectByType<Terrain>();
        this.gridManager = GetComponentInChildren<GridManager>();
    }
    private void OnEnable()
    {
        this.patrolPointParent.SetParent(null);
        StartCoroutine(PatrolRoutine());
    }
    WaitForSeconds wsForAttack = new WaitForSeconds(1);
    [SerializeField]
    List<GridManager.Node> path;
    IEnumerator PatrolRoutine()
    {
        while (!this.gridManager.IsGridSet)
        {
            yield return null; // grid셋팅까지 대기
        }
        // 포인트 하나씩 올려가면서 순찰하다가,
        // 앞쪽에 바닥 콜라이더 감지되면 바로 다음 포인트로
        while (true) // 플레이어가 주변에 들어올때까지
        {
            while (Vector3.Distance(this.transform.position, patrolPoints[idx].position) > 1f)
            {
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(patrolPoints[idx].position - this.transform.position), Time.deltaTime * 10);
                this.transform.Translate(Vector3.forward * Time.deltaTime * this.speed);
                // 앞쪽에 바닥 콜라이더 감지되는지 확인
                if (Physics.Raycast(this.transform.position, this.transform.forward, 4f , 1 << 7))
                {
                    this.idx = (this.idx + 1) % patrolPoints.Length;
                    this.transform.LookAt(patrolPoints[idx]);
                    continue;
                }
                // 타겟이 근처에 왔는지 확인
                if(Vector3.Distance(this.target.position,this.transform.position) < this.searchDist) // 포착 조건
                {
                    this.speed = 200f;
                    while(Vector3.Distance(this.target.position, this.transform.position) < this.chaseDist) // 한번 포착된 다음 따라가는 조건
                    {
                        // 공격 관련 로직
                        if(Vector3.Distance(this.target.position,this.transform.position) < this.attackDist) // 공격 가능 거리 조건
                        {
                            yield return this.wsForAttack;
                        }
                        else
                        {
                            this.transform.LookAt(this.target.position);
                            if (Physics.Raycast(this.transform.position, this.transform.forward, 
                                Vector3.Distance(this.transform.position,this.target.position), 1 << 7)) //타겟과 상어 사이에 장애물이 있다면
                            {
                                //var path = this.gridManager.FindPath(this.transform.position, this.target.position);
                                this.path = this.gridManager.FindPath(this.transform.position, this.target.position);
                                Debug.Log(this.target.position);
                                int index = 0;
                                while (Physics.Raycast(this.transform.position, (this.target.position - this.transform.position).normalized, 
                                    Vector3.Distance(this.transform.position, this.target.position), 1 << 7)) 
                                    // 상어와 타겟 사이에 장애물이 있는 동안만
                                {
                                    if (path == null || index >= path.Count)
                                    {
                                        break;
                                    }
                                    

                                    Vector3 targetPoint = path[index].worldPosition;
                                    this.transform.LookAt(targetPoint);
                                    this.transform.Translate(Vector3.forward * Time.deltaTime * speed);

                                    if (Vector3.Distance(transform.position, targetPoint) < this.gridManager.nodeRadius)
                                        index++;
                                    yield return null;
                                }
                            }
                            else
                            {
                                this.transform.Translate(Vector3.forward * Time.deltaTime * this.speed); //없으면 그대로 직진
                            }

                                yield return null;
                        }
                    }
                    break; // 순찰 지점을 향해 LookAt하는게 while밖에 도입부에 있기에 break로 나가고 순찰포인트 변경
                }
                this.speed = 50;
                yield return null;
            }
            this.idx = (this.idx + 1) % patrolPoints.Length;

        }
    }
    private void Update()
    {
        if (test)
        {
            test = false;
            DisActive();
        }
    }
    private void DisActive()
    {
        this.patrolPointParent.SetParent(this.transform); // 비활성화될때 비활성화하기전에 다시 집어넣고 비활성화
                                                          // OnDisable에서 부모 건드리는 코드가 안들어감
        this.patrolPointParent.localPosition = Vector3.zero;
        this.gameObject.SetActive(false);
    }
}
