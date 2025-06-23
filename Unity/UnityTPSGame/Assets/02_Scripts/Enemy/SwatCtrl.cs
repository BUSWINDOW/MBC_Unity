using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwatCtrl : MonoBehaviour
{
    int hp;
    private SwatAI ai;
    private SwatDamage damage;
    private SwatFire e_Fire;
    private MoveAgent moveAgent;
    private SwatAnimationCtrl animCtrl;

    private TPSPlayerCtrl player;

    private bool isDie;
    private WaitForSeconds ws = new WaitForSeconds(3);
    
    void Awake()
    {

        

        this.hp = 30;
        this.ai = GetComponent<SwatAI>();
        this.damage = GetComponent<SwatDamage>();
        this.moveAgent = GetComponent<MoveAgent>();

        this.damage.hitAction = (dmg) =>
        {
            this.hp -= dmg;
            if (this.hp <= 0)
            {
                this.hp = 0;
                this.isDie = true;
                this.ai.state = EnemyAI.eState.Die;
                this.Die();
                
            }
        };

        

        this.e_Fire = GetComponent<SwatFire>();
        this.animCtrl = GetComponent<SwatAnimationCtrl>();
    }

    private void OnEnable()
    {
        TPSPlayerCtrl.onPlayerDie += this.PlayerDie;
        BarrelCtrl.explodAction += this.Die;
    }
    private void OnDisable()
    {
        TPSPlayerCtrl.onPlayerDie -= this.PlayerDie;
        BarrelCtrl.explodAction -= this.Die;

    }


    IEnumerator DieRoutine()
    {
        yield return ws;
        this.gameObject.SetActive(false);
        this.hp = 30;
        this.isDie = false;
        this.animCtrl.SetPatroll(this.moveAgent.Speed);
        this.GetComponent<CapsuleCollider>().enabled = true;
        this.ai.state = EnemyAI.eState.Patrol;
        this.ai.isDie = false;
    }
    private void Die()
    {
        //Debug.Log("Die");
        this.ai.isDie = true;
        this.e_Fire.isFire = false;
        this.e_Fire.isReload = false;
        this.moveAgent.agent.isStopped = true;
        this.animCtrl.SetDie();
        this.GetComponent<CapsuleCollider>().enabled = false;

        StartCoroutine(this.DieRoutine());
    }

    public void PlayerDie()
    {
        this.e_Fire.isFire = false;
        this.moveAgent.Stop();
        this.ai.StopAllCoroutines();
        this.animCtrl.SetGameOver();
    }
}
