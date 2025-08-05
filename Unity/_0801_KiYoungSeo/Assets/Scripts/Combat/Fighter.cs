using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using UnityEngine.InputSystem.iOS;
namespace RPG.Combat
{
    public class Fighter : MonoBehaviour,IAction
    {
        //타겟이 있으면, 해당 타겟 위치로 Mover를 이용해 이동, 가까워지면 이동 중지후 공격
        Mover mover;
        float attackDist = 1;
        Animator anim;

        private readonly int hashAttack = Animator.StringToHash("Attack");
        private readonly int hashStopAttack = Animator.StringToHash("stopAttack");
        public void Init()
        {
            this.mover = GetComponent<Mover>();
            this.mover.Init();
            this.anim = GetComponent<Animator>();
        }
        public void AttackSet(GameObject target)
        {
            StartCoroutine(this.AttackRoutine(target));
        }
        WaitForSeconds wsForAttack = new WaitForSeconds(1);
        IEnumerator AttackRoutine(GameObject target)
        {
            while (true)
            {
                while (Vector3.Distance(target.transform.position, this.transform.position) > this.attackDist)
                {
                    this.mover.MoveSet(target.transform.position, 5.5f);
                    yield return null;
                }
                //충분히 가까워져서 while문을 빠져나왔을 때
                this.mover.Cancel();
                while (Vector3.Distance(target.transform.position, this.transform.position) <= this.attackDist && !target.GetComponent<Health>().isDie)
                {
                    this.anim.SetTrigger(hashAttack);
                    target.GetComponent<CombatTarget>().GetHit(20);
                    yield return this.wsForAttack;
                }
                if (target.GetComponent<Health>().isDie)
                {
                    break;
                }
            }
            
        }
        public void Cancel()
        {
            this.StopAllCoroutines();
            this.anim.SetTrigger(hashStopAttack);
            this.mover.Cancel();
        }
    }

}

