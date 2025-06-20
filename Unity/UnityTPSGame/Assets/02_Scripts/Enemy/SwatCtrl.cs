using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwatCtrl : MonoBehaviour
{
    int hp;
    private SwatAI ai;
    private SwatDamage damage;
    private SwatFire fire;
    private SwatAnimationCtrl anim;

    private bool isDie;

    void Awake()
    {
        this.hp = 30;
        this.ai = GetComponent<SwatAI>();
        this.damage = GetComponent<SwatDamage>();

        this.damage.hitAction = (dmg) =>
        {
            this.hp -= dmg;
            if (this.hp <= 0)
            {
                this.hp = 0;
                this.isDie = true;
                this.ai.state = EnemyAI.eState.Die;
            }
        };

        this.fire = GetComponent<SwatFire>();
        this.anim = GetComponent<SwatAnimationCtrl>();
    }

}
