using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : Health
    {
        Mover mover;
        Fighter fighter;
        PatrolPoint patrolPointCtrl;
        [SerializeField]GameObject target;
        float attackDist = 5;
        int patrolIdx = -1; // 순찰 컨트롤이 계속 1을 더해줄거기 때문에 -1로 시작함
        float lookAroundTime = 0;
        void Awake()
        {
            this.target = GameObject.Find("PlayerParent").GetComponentInChildren<PlayerController>(true).gameObject;
            this.mover = GetComponent<Mover>();
            this.mover.Init();
            this.fighter = GetComponent<Fighter>();
            this.fighter.Init();
            this.anim = GetComponent<Animator>();
            this.patrolPointCtrl = GameObject.Find("PatrolPoints").GetComponent<PatrolPoint>();

            
        }
        private void OnEnable()
        {
            this.isDie = false;
            StartCoroutine(EnemyRoutine());
        }
        IEnumerator EnemyRoutine()
        {
            this.mover.MoveSet(this.patrolPointCtrl.GetPoint(ref this.patrolIdx) , 3.5f);

            while (true)
            {
                while (Vector3.Distance(this.target.transform.position, this.transform.position) > attackDist || target.GetComponent<PlayerController>().isDie)
                {
                    if (this.mover.GetDistance() < 1)
                    {
                        //도착하면 3초간 머물렀다가 이동
                        this.lookAroundTime = 0;
                        while ((Vector3.Distance(this.target.transform.position, this.transform.position) > attackDist || target.GetComponent<PlayerController>().isDie) && this.lookAroundTime < 3)
                        {
                            //안가까워지는 동안만, 3초 동안만 제자리에 서있음
                            this.lookAroundTime += Time.deltaTime;
                            yield return null;
                        }
                        this.mover.MoveSet(this.patrolPointCtrl.GetPoint(ref this.patrolIdx), 3.5f);
                    }
                    yield return null;
                }
                //타겟이 가까워져서 여기로 나왔다면
                this.fighter.AttackSet(this.target); // 공격 셋팅
                while (Vector3.Distance(this.target.transform.position, this.transform.position) <= attackDist && !target.GetComponent<PlayerController>().isDie)
                {
                    yield return null;
                }
                //다시 멀어지거나, 죽거나
                this.fighter.Cancel();
                this.lookAroundTime = 0;
                while ((Vector3.Distance(this.target.transform.position, this.transform.position) > attackDist || target.GetComponent<PlayerController>().isDie) && this.lookAroundTime < 3)
                {
                    //다시 안가까워지는 동안만, 3초 동안만 제자리에 서있음
                    this.lookAroundTime += Time.deltaTime;
                    yield return null;
                }
            }
        }
        public override void Die()
        {
            base.Die();
            this.fighter.Cancel();
            this.isDie = true;
            StopAllCoroutines();
            
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, 5);
            
        }
    }
}