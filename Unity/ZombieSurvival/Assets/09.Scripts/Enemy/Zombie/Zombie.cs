using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie :LivingEntity
{
    private NavMeshAgent agent; // 네비게이션 에이전트
    private AudioSource source;
    private Animator animator;
    private MeshRenderer meshRenderer; // 메쉬 랜더러

    //오디오소스, 네브메쉬, 콜라이더, 오디오클립, blood Effect, 랜더러

    public LayerMask targetLayer; // 타겟 엔티티 레이어 마스크
    private LivingEntity targetEntity; // 타겟 엔티티

    public ParticleSystem hitEffect; // 피격 이팩트
    public AudioClip hitClip; // 피격 소리
    public AudioClip deathClip; // 죽음 소리

    public int damage = 20;
    public float timeBetweenAttack = 0.5f; // 공격 간격
    private float lastAttackTime; // 마지막 공격 시간

    private readonly int hashHasTarget = Animator.StringToHash("HasTarget"); // 애니메이터 해시
    private readonly int hashDie = Animator.StringToHash("Die"); // 애니메이터 해시


    //추적 대상이 있는지 알려주는 프로퍼티
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

    public void Setup(ZombieData data) //좀비의 스펙을 결정하는 함수(이동속도, 체력 등)
    {
        this.maxHp = data.hp; // 좀비의 최대 체력 설정
        this.hp = this.maxHp;
        this.damage = data.damage; // 좀비의 공격력 설정
        this.agent.speed = data.moveSpeed; // 네비게이션 에이전트의 이동 속도 설정
        this.meshRenderer.material.color = data.skinColor; // 좀비의 색상 설정
    }
    private void Start()
    {
        StartCoroutine(this.UpdatePath()); // 경로 업데이트 코루틴 시작
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
                this.agent.SetDestination(this.targetEntity.transform.position); // 타겟 엔티티의 위치로 경로 설정
            }
            else
            {
                this.agent.isStopped = true; // 타겟이 없으면 이동 중지
                Collider[] cols = Physics.OverlapSphere(transform.position, 10f, this.targetLayer); // 주변의 타겟 엔티티 검색
                for (int i = 0; i < cols.Length; i++)
                {
                    LivingEntity entity = cols[i].GetComponent<LivingEntity>();
                    if (entity != null && !entity.isDead)
                    {
                        this.targetEntity = entity; // 타겟 엔티티 설정
                        break; // 첫 번째 타겟을 찾으면 루프 종료
                    }
                }
            }
            yield return this.pathUpdateTime; // 0.25초마다 경로 업데이트
        }
    }
    public override void OnDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!isDead)
        {
            this.hitEffect.transform.position = hitPoint; // 피격 이펙트 위치 설정
            this.hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal); // 피격 이펙트 회전 설정
            this.hitEffect.Play(); // 피격 이펙트 재생
            this.source.PlayOneShot(this.hitClip); // 피격 소리 재생
        }
        base.OnDamage(damage, hitPoint, hitNormal);
    }
    public override void Die()
    {
        base.Die();
        //자신의 모든 콜라이더 비활성화
        Collider[] cols = GetComponents<Collider>(); // 콜라이더 가져오기
        foreach (Collider col in cols)
        {
            col.enabled = false; // 콜라이더 비활성화
        }
        this.agent.isStopped = true; // 네비게이션 에이전트 정지
        this.agent.enabled = false; // 네비게이션 에이전트 비활성화
        this.source.PlayOneShot(this.deathClip); // 죽음 소리 재생
        this.animator.SetTrigger(hashDie); // 죽음 애니메이션 트리거
    }
    public void OnTriggerStay(Collider other)
    {
        //트리거가 충돌한게 추적 대상이라면 공격 실행
        if (!this.isDead && Time.time >= this.lastAttackTime + timeBetweenAttack)
        {
            LivingEntity entity = other.GetComponent<LivingEntity>();
            if(entity != null && this.targetEntity == entity)
            {
                this.lastAttackTime = Time.time; // 마지막 공격 시간 갱신
                Vector3 hitPos = other.ClosestPoint(this.transform.position);
                //상대방 피격 위치와 피격 방향을 근갓값으로 계산
                Vector3 hitNormal = (this.transform.position - other.transform.position);
                // 공격되는 방향을 계산

                entity.OnDamage(this.damage, hitPos, hitNormal.normalized); // 공격 실행

            }
        }
        
    }
}
