using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie :LivingEntity
{
    private NavMeshAgent agent; // �׺���̼� ������Ʈ
    private AudioSource source;
    private Animator animator;
    private MeshRenderer meshRenderer; // �޽� ������

    //������ҽ�, �׺�޽�, �ݶ��̴�, �����Ŭ��, blood Effect, ������

    public LayerMask targetLayer; // Ÿ�� ��ƼƼ ���̾� ����ũ
    private LivingEntity targetEntity; // Ÿ�� ��ƼƼ

    public ParticleSystem hitEffect; // �ǰ� ����Ʈ
    public AudioClip hitClip; // �ǰ� �Ҹ�
    public AudioClip deathClip; // ���� �Ҹ�

    public int damage = 20;
    public float timeBetweenAttack = 0.5f; // ���� ����
    private float lastAttackTime; // ������ ���� �ð�

    private readonly int hashHasTarget = Animator.StringToHash("HasTarget"); // �ִϸ����� �ؽ�
    private readonly int hashDie = Animator.StringToHash("Die"); // �ִϸ����� �ؽ�


    //���� ����� �ִ��� �˷��ִ� ������Ƽ
    private bool hasTarget
    {
        get
        {
            return this.targetEntity != null && !this.targetEntity.isDead;
        }
    }

    void Awake()
    {
        this.source = GetComponent<AudioSource>();
        this.agent = GetComponent<NavMeshAgent>();
        this.animator = GetComponent<Animator>();
        this.meshRenderer = GetComponent<MeshRenderer>();

    }

    public void Setup(ZombieData data) //������ ������ �����ϴ� �Լ�(�̵��ӵ�, ü�� ��)
    {
        this.maxHp = data.hp; // ������ �ִ� ü�� ����
        this.hp = this.maxHp;
        this.damage = data.damage; // ������ ���ݷ� ����
        this.agent.speed = data.moveSpeed; // �׺���̼� ������Ʈ�� �̵� �ӵ� ����
        this.meshRenderer.material.color = data.skinColor; // ������ ���� ����
    }
    private void Start()
    {
        StartCoroutine(this.UpdatePath()); // ��� ������Ʈ �ڷ�ƾ ����
    }
    void Update()
    {
        this.animator.SetBool(this.hashHasTarget,this.hasTarget);
    }
    private WaitForSeconds pathUpdateTime = new WaitForSeconds(0.25f);
    IEnumerator UpdatePath()
    {
        while (!this.isDead)
        {
            if (hasTarget)
            {
                this.agent.isStopped = false;
                this.agent.SetDestination(this.targetEntity.transform.position); // Ÿ�� ��ƼƼ�� ��ġ�� ��� ����
            }
            else
            {
                this.agent.isStopped = true; // Ÿ���� ������ �̵� ����
                Collider[] cols = Physics.OverlapSphere(transform.position, 10f, this.targetLayer); // �ֺ��� Ÿ�� ��ƼƼ �˻�
                for (int i = 0; i < cols.Length; i++)
                {
                    LivingEntity entity = cols[i].GetComponent<LivingEntity>();
                    if (entity != null && !entity.isDead)
                    {
                        this.targetEntity = entity; // Ÿ�� ��ƼƼ ����
                        break; // ù ��° Ÿ���� ã���� ���� ����
                    }
                }
            }
            yield return this.pathUpdateTime; // 0.25�ʸ��� ��� ������Ʈ
        }
    }
    public override void OnDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!isDead)
        {
            this.hitEffect.transform.position = hitPoint; // �ǰ� ����Ʈ ��ġ ����
            this.hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal); // �ǰ� ����Ʈ ȸ�� ����
            this.hitEffect.Play(); // �ǰ� ����Ʈ ���
            this.source.PlayOneShot(this.hitClip); // �ǰ� �Ҹ� ���
        }
        base.OnDamage(damage, hitPoint, hitNormal);
    }
    public override void Die()
    {
        base.Die();
        //�ڽ��� ��� �ݶ��̴� ��Ȱ��ȭ
        Collider[] cols = GetComponents<Collider>(); // �ݶ��̴� ��������
        foreach (Collider col in cols)
        {
            col.enabled = false; // �ݶ��̴� ��Ȱ��ȭ
        }
        this.agent.isStopped = true; // �׺���̼� ������Ʈ ����
        this.agent.enabled = false; // �׺���̼� ������Ʈ ��Ȱ��ȭ
        this.source.PlayOneShot(this.deathClip); // ���� �Ҹ� ���
        this.animator.SetTrigger(hashDie); // ���� �ִϸ��̼� Ʈ����
    }
    public void OnTriggerStay(Collider other)
    {
        //Ʈ���Ű� �浹�Ѱ� ���� ����̶�� ���� ����
        if (!this.isDead && Time.time >= this.lastAttackTime + timeBetweenAttack)
        {
            LivingEntity entity = other.GetComponent<LivingEntity>();
            if(entity != null && this.targetEntity == entity)
            {
                this.lastAttackTime = Time.time; // ������ ���� �ð� ����
                Vector3 hitPos = other.ClosestPoint(this.transform.position);
                //���� �ǰ� ��ġ�� �ǰ� ������ �ٰ������� ���
                Vector3 hitNormal = (this.transform.position - other.transform.position);
                // ���ݵǴ� ������ ���

                entity.OnDamage(this.damage, hitPos, hitNormal.normalized); // ���� ����

            }
        }
        
    }
}
