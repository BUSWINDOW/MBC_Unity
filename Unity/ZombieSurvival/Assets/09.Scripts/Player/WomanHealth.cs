using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WomanHealth : LivingEntity
{
    public Slider healthSlider; // 체력 슬라이더 UI
    public AudioClip deathClip; // 죽는 소리
    public AudioClip damageClip; // 데미지 받는 소리
    public AudioClip itemGetClip; // 아이템 획득 소리

    public AudioSource audioSource; // 오디오 소스
    private WomanMovement movement; // 여성 캐릭터의 이동 스크립트
    private WomanShooter shooter; // 여성 캐릭터의 슈팅 스크립트
    private Animator anim; // 애니메이션 컴포넌트

    private readonly int deathTrigger = Animator.StringToHash("Die"); // 죽는 애니메이션 트리거 해시

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
        // 추가적인 초기화 작업이 필요하다면 여기에 작성
        this.healthSlider.gameObject.SetActive(true); // 체력 슬라이더 UI 활성화

        this.healthSlider.maxValue = this.maxHp; // 체력 슬라이더 최대값 설정
        this.healthSlider.value = this.hp; // 체력 슬라이더 초기화

        this.movement.enabled = true; // 이동 스크립트 활성화
        this.shooter.enabled = true; // 슈팅 스크립트 활성화
    }
    public override void OnDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (this.isDead) return; // 이미 죽은 상태라면 데미지 처리하지 않음
        this.audioSource.PlayOneShot(this.damageClip); // 데미지 받는 소리 재생
        base.OnDamage(damage, hitPoint, hitNormal);
        // 추가적인 데미지 처리 로직이 필요하다면 여기에 작성
        this.healthSlider.value = this.hp; // 체력 슬라이더 업데이트

    }
    public override void RestoreHealth(int amount)
    {
        base.RestoreHealth(amount);
        // 추가적인 체력 회복 로직이 필요하다면 여기에 작성
        this.healthSlider.value = this.hp; // 체력 슬라이더 업데이트
    }
    public override void Die()
    {
        base.Die();
        // 추가적인 죽음 처리 로직이 필요하다면 여기에 작성
        // 예: 사운드 재생, 애니메이션 트리거 등
        this.healthSlider.gameObject.SetActive(false); // 체력 슬라이더 UI 비활성화
        this.audioSource.PlayOneShot(this.deathClip); // 죽는 소리 재생
        this.anim.SetTrigger(this.deathTrigger); // 죽는 애니메이션 트리거
        this.movement.enabled = false; // 이동 스크립트 비활성화 
        this.shooter.enabled = false; // 슈팅 스크립트 비활성화
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!this.isDead)
        {
            IItem item = other.GetComponent<IItem>();
            if (item != null) 
            {
                item.Use(this.gameObject); // 아이템 사용
                this.audioSource.PlayOneShot(this.itemGetClip); // 아이템 획득 소리 재생
            }

        }
    }
}

