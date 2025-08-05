using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(_Test_Mover))]
public class _Test_Fighter : MonoBehaviour,_Test_IAction
{
    //타겟이 있으면, 해당 타겟 위치로 Mover를 이용해 이동, 가까워지면 이동 중지후 공격
    _Test_Mover mover;
    float attackDist = 1;
    void Start()
    {
        this.mover = GetComponent<_Test_Mover>();
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
                this.mover.MoveSet(target.transform.position);
                yield return null;
            }
            //충분히 가까워져서 while문을 빠져나왔을 때
            this.mover.Cancel();
            while (Vector3.Distance(target.transform.position, this.transform.position) <= this.attackDist)
            {
                Debug.Log("공격");
                yield return this.wsForAttack;
            }
        }
    }
    public void Cancel()
    {
        this.StopAllCoroutines();
        this.mover.Cancel();
    }
}
