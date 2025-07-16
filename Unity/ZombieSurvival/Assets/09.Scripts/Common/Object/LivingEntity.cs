using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LivingEntity : MonoBehaviourPun,IDamageable
{
    public int maxHp = 100; // 최대 체력
    public int hp { get; protected set; } // 현재 체력
    public bool isDead { get; protected set; } // 생존 여부
    public Action DieAction; // 죽음 이벤트 액션

    [PunRPC]
    public void ApplyUpdatedHealth(int newHealth, bool newDead)
    {
        this.hp = newHealth;
        this.isDead = newDead;
    }

    protected virtual void OnEnable()
    {
        this.isDead = false;
        this.hp = this.maxHp;
    }

    [PunRPC]
    public virtual void OnDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        //체력 있어야함
        //체력이 0이 됐을때 Action 실행
        if (PhotonNetwork.IsMasterClient)
        {
            this.hp -= damage; // 데미지 적용
            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, this.hp, this.isDead); // 다른 클라이언트에게 체력 업데이트 전송
            photonView.RPC("OnDamage", RpcTarget.Others, damage, hitPoint, hitNormal); // Die 실행 시키려고 넣은거같은데 Die만 따로 되어있는거보면
                                                                                       // 체력 업데이트는 마스터쪽에서 다 같이 적용될거고 OnDamage를
                                                                                       // 다른 클라이언트에서 실행 시키면, Master if문은 안들어가고 아래
                                                                                       // Die쪽만 들어갈거니까
        }

        
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
    [PunRPC]
    public virtual void RestoreHealth(int amount)
    {
        if (this.isDead)
        {
            return; // 이미 죽은 상태라면 체력 회복 불가
        }

        if (PhotonNetwork.IsMasterClient)
        {
            this.hp += amount; // 체력 회복
            this.hp = Mathf.Min(this.hp, this.maxHp); // 최대 체력 초과 방지

            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, this.hp, this.isDead); // 다른 클라이언트에게 체력 업데이트 전송
            //photonView.RPC("RestoreHealth", RpcTarget.Others, amount); // 다른 클라이언트에게 체력 회복 알림
        }
        
    }

}
