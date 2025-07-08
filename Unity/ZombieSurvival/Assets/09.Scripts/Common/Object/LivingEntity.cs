using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour,IDamageable
{
    public int maxHp = 100; // 최대 체력
    public int hp { get; protected set; } // 현재 체력
    public bool isDead { get; protected set; } // 생존 여부
    public Action DieAction; // 죽음 이벤트 액션


    protected virtual void OnEnable()
    {
        this.isDead = false;
        this.hp = this.maxHp;
    }
    public virtual void OnDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        //체력 있어야함
        //체력이 0이 됐을때 Action 실행

        this.hp -= damage; // 데미지 적용
        if(this.hp <= 0&&!this.isDead)//아직 죽지 않은 상태에서 체력이 0 이하가 되었을때
        {
            Die();
        }
    }

    public virtual void Die()
    {
        this.isDead = true; // 죽음 상태로 변경
        this.DieAction();

        this.gameObject.SetActive(false); // 오브젝트 비활성화
    }
    public virtual void RestoreHealth(int amount)
    {
        if (this.isDead)
        {
            return; // 이미 죽은 상태라면 체력 회복 불가
        }
        this.hp += amount; // 체력 회복
        this.hp = Mathf.Min(this.hp, this.maxHp); // 최대 체력 초과 방지
    }

}
