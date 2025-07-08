using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour,IDamageable
{
    public int maxHp = 100; // �ִ� ü��
    public int hp { get; protected set; } // ���� ü��
    public bool isDead { get; protected set; } // ���� ����
    public Action DieAction; // ���� �̺�Ʈ �׼�


    protected virtual void OnEnable()
    {
        this.isDead = false;
        this.hp = this.maxHp;
    }
    public virtual void OnDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        //ü�� �־����
        //ü���� 0�� ������ Action ����

        this.hp -= damage; // ������ ����
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
    public virtual void RestoreHealth(int amount)
    {
        if (this.isDead)
        {
            return; // �̹� ���� ���¶�� ü�� ȸ�� �Ұ�
        }
        this.hp += amount; // ü�� ȸ��
        this.hp = Mathf.Min(this.hp, this.maxHp); // �ִ� ü�� �ʰ� ����
    }

}
