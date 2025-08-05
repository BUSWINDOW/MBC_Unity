using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Test_EnemyCtrl : MonoBehaviour
{
    _Test_Mover mover;
    _Test_Fighter fighter;
    _Test_PatrolPointCtrl patrolPointCtrl;
    GameObject target;
    float attackDist = 5;
    int patrolIdx = -1; // 순찰 컨트롤이 계속 1을 더해줄거기 때문에 -1로 시작함
    float lookAroundTime = 0;
    void Start()
    {
        this.mover = GetComponent<_Test_Mover>();
        this.fighter = GetComponent<_Test_Fighter>();
        this.target = GameObject.FindWithTag("Player");
        this.patrolPointCtrl = GameObject.Find("PatrolPoints").GetComponent<_Test_PatrolPointCtrl>();

        StartCoroutine(EnemyRoutine());
    }

    IEnumerator EnemyRoutine()
    {
        this.mover.MoveSet(this.patrolPointCtrl.GetPoint(ref this.patrolIdx));
        
        while (true)
        {
            while (Vector3.Distance(this.target.transform.position, this.transform.position) > attackDist)
            {
                if (this.mover.GetDistance() < 1)
                {
                    this.mover.MoveSet(this.patrolPointCtrl.GetPoint(ref this.patrolIdx));
                }
                yield return null;
            }
            //타겟이 가까워져서 여기로 나왔다면
            this.fighter.AttackSet(this.target); // 공격 셋팅
            while (Vector3.Distance(this.target.transform.position, this.transform.position) <= attackDist)
            {
                yield return null;
            }
            //다시 멀어진다면(추가적인 조건 셋팅 해야함, 타겟이 죽는다거나)
            this.fighter.Cancel();
            this.lookAroundTime = 0;
            while(Vector3.Distance(this.target.transform.position, this.transform.position) > attackDist && this.lookAroundTime < 2)
            {
                //다시 안가까워지는 동안만, 2초 동안만 제자리에 서있음
                this.lookAroundTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}
