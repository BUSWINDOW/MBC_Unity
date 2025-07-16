using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;

public class WomanHealth : LivingEntity
{
    public Slider healthSlider; // ü�� �����̴� UI
    public AudioClip deathClip; // �״� �Ҹ�
    public AudioClip damageClip; // ������ �޴� �Ҹ�
    public AudioClip itemGetClip; // ������ ȹ�� �Ҹ�

    public AudioSource audioSource; // ����� �ҽ�
    private WomanMovement movement; // ���� ĳ������ �̵� ��ũ��Ʈ
    private WomanShooter shooter; // ���� ĳ������ ���� ��ũ��Ʈ
    private Animator anim; // �ִϸ��̼� ������Ʈ

    private readonly int deathTrigger = Animator.StringToHash("Die"); // �״� �ִϸ��̼� Ʈ���� �ؽ�

    //public Action DieAction; // ���� �̺�Ʈ �׼�

    private void Awake()
    {
        this.audioSource = GetComponent<AudioSource>();
        this.movement = GetComponent<WomanMovement>();
        this.shooter = GetComponent<WomanShooter>();
        this.anim = GetComponent<Animator>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        // �߰����� �ʱ�ȭ �۾��� �ʿ��ϴٸ� ���⿡ �ۼ�
        this.healthSlider.gameObject.SetActive(true); // ü�� �����̴� UI Ȱ��ȭ

        this.healthSlider.maxValue = this.maxHp; // ü�� �����̴� �ִ밪 ����
        this.healthSlider.value = this.hp; // ü�� �����̴� �ʱ�ȭ

        this.movement.enabled = true; // �̵� ��ũ��Ʈ Ȱ��ȭ
        this.shooter.enabled = true; // ���� ��ũ��Ʈ Ȱ��ȭ
    }
    [PunRPC]
    public override void OnDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (this.isDead) return; // �̹� ���� ���¶�� ������ ó������ ����
        this.audioSource.PlayOneShot(this.damageClip); // ������ �޴� �Ҹ� ���
        base.OnDamage(damage, hitPoint, hitNormal);
        // �߰����� ������ ó�� ������ �ʿ��ϴٸ� ���⿡ �ۼ�
        this.healthSlider.value = this.hp; // ü�� �����̴� ������Ʈ

    }
    [PunRPC]
    public override void RestoreHealth(int amount)
    {
        base.RestoreHealth(amount);
        // �߰����� ü�� ȸ�� ������ �ʿ��ϴٸ� ���⿡ �ۼ�
        this.healthSlider.value = this.hp; // ü�� �����̴� ������Ʈ
    }
    public override void Die()
    {
        base.Die();
        //DieAction();
        // �߰����� ���� ó�� ������ �ʿ��ϴٸ� ���⿡ �ۼ�
        // ��: ���� ���, �ִϸ��̼� Ʈ���� ��
        this.healthSlider.gameObject.SetActive(false); // ü�� �����̴� UI ��Ȱ��ȭ
        this.audioSource.PlayOneShot(this.deathClip); // �״� �Ҹ� ���
        this.anim.SetTrigger(this.deathTrigger); // �״� �ִϸ��̼� Ʈ����
        this.movement.enabled = false; // �̵� ��ũ��Ʈ ��Ȱ��ȭ 
        this.shooter.enabled = false; // ���� ��ũ��Ʈ ��Ȱ��ȭ

        Invoke("Respawn", 5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!this.isDead)
        {
            IItem item = other.GetComponent<IItem>();
            if (item != null)
            {
                //ȣ��Ʈ �ʿ��� ������ ȹ�� ó��
                if (PhotonNetwork.IsMasterClient)
                {
                    item.Use(this.gameObject); // ������ ���
                }

                this.audioSource.PlayOneShot(this.itemGetClip); // ������ ȹ�� �Ҹ� ���
            }

        }
    }

    public void Respawn()
    {
        if (photonView.IsMine)
        {
            this.transform.position = Vector3.zero; // ������ ��ġ ���� (��: 0,0,0)
        }

        gameObject.SetActive(false); // ���� ������Ʈ ��Ȱ��ȭ
        gameObject.SetActive(true); // �ٽ� Ȱ��ȭ�Ͽ� ������ ȿ��
        //OnDisable()�� OnEnable()�� �ڵ����� ȣ���
    }
}

