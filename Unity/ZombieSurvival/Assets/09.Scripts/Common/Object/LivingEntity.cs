using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LivingEntity : MonoBehaviourPun,IDamageable
{
    public int maxHp = 100; // �ִ� ü��
    public int hp { get; protected set; } // ���� ü��
    public bool isDead { get; protected set; } // ���� ����
    public Action DieAction; // ���� �̺�Ʈ �׼�

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
        //ü�� �־����
        //ü���� 0�� ������ Action ����
        if (PhotonNetwork.IsMasterClient)
        {
            this.hp -= damage; // ������ ����
            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, this.hp, this.isDead); // �ٸ� Ŭ���̾�Ʈ���� ü�� ������Ʈ ����
            photonView.RPC("OnDamage", RpcTarget.Others, damage, hitPoint, hitNormal); // Die ���� ��Ű���� �����Ű����� Die�� ���� �Ǿ��ִ°ź���
                                                                                       // ü�� ������Ʈ�� �������ʿ��� �� ���� ����ɰŰ� OnDamage��
                                                                                       // �ٸ� Ŭ���̾�Ʈ���� ���� ��Ű��, Master if���� �ȵ��� �Ʒ�
                                                                                       // Die�ʸ� ���Ŵϱ�
        }

        
        if(this.hp <= 0&&!this.isDead)//���� ���� ���� ���¿��� ü���� 0 ���ϰ� �Ǿ�����
        {
            Die();
        }
    }

    public virtual void Die()
    {
        this.isDead = true; // ���� ���·� ����
        this.DieAction();

        this.gameObject.SetActive(false); // ������Ʈ ��Ȱ��ȭ
    }
    [PunRPC]
    public virtual void RestoreHealth(int amount)
    {
        if (this.isDead)
        {
            return; // �̹� ���� ���¶�� ü�� ȸ�� �Ұ�
        }

        if (PhotonNetwork.IsMasterClient)
        {
            this.hp += amount; // ü�� ȸ��
            this.hp = Mathf.Min(this.hp, this.maxHp); // �ִ� ü�� �ʰ� ����

            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, this.hp, this.isDead); // �ٸ� Ŭ���̾�Ʈ���� ü�� ������Ʈ ����
            //photonView.RPC("RestoreHealth", RpcTarget.Others, amount); // �ٸ� Ŭ���̾�Ʈ���� ü�� ȸ�� �˸�
        }
        
    }

}
