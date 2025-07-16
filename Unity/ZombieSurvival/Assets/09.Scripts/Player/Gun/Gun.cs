using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviourPun, IPunObservable
{
    public enum eState
    {
        Ready, // 대기 상태
        Fire, // 사격 중
        Reload // 장전 중
    }
    //public eState state = eState.Ready; // 총의 현재 상태

    public eState State { get; private set; } = eState.Ready; // 총의 현재 상태 (자동 프로퍼티로 변경)
    //총의 현재 상태를 나타내는 변수, 기본값은 Ready
    public Transform firePos; // 총알이 발사되는 위치
    public ParticleSystem muzzleFlashEffect; // 총구 플래시 이펙트
    public ParticleSystem shellEffect; // 탄피 이펙트

    private LineRenderer lineRenderer; // 총알 궤적을 그리기 위한 라인 렌더러

    public GunData gunData; // 총 데이터 스크립터블 오브젝트

    private float fireDistance = 100f; // 총알이 날아가는 최대 거리(사정거리)

    public int ammo { get; set; } // 현재 보유중인 총알 수
    public int magAmmo { get; private set; } // 현재 탄창 내 총알 수

    public float lastShotTime = 0f; // 마지막 사격 시간
    private Vector3 hitPos; // 총알이 맞은 위치

    private AudioSource source;

    public Action reloadAnimationAction;

    
    void Awake()
    {
        this.source = GetComponent<AudioSource>();

        this.lineRenderer = GetComponent<LineRenderer>();
        this.lineRenderer.positionCount = 2; // 라인 렌더러의 포지션 개수를 2로 설정 (시작점과 끝점)
        this.lineRenderer.enabled = false; // 초기에는 라인 렌더러 비활성화

        this.ammo = gunData.maxAmmo; // 총알 수를 초기화
        this.magAmmo = gunData.magCapacity; // 탄창 내 총알 수를 초기화
        this.ammo -= this.magAmmo; // 초기화 후 남은 총알 수 계산

        this.shotEffectTime = new WaitForSeconds(0.03f);
        this.reloadTime = new WaitForSeconds(gunData.reloadTime); // 장전 시간 초기화
    }

    private void Start()
    {
        photonView.Synchronization = ViewSynchronization.ReliableDeltaCompressed;
        //데이터가 중요한 내용이므로 TCP/IP로 설정
        photonView.ObservedComponents[0] = this; // 이미 들어가있긴하지만, 코드로 그냥 써둔것
    }

    private void OnEnable()
    {
        // 플레이어 쪽 총 내용을 이 총 데이터로 변경
        this.State = eState.Ready; // 총의 상태를 Ready로 초기화
        this.lastShotTime = 0f; // 마지막 사격 시간을 초기화
    }
    public void Fire()
    {
        // 발사
        if (this.State == eState.Ready && Time.time >= this.lastShotTime + this.gunData.timeBetweenShots)
        {
            this.lastShotTime = Time.time;
            this.Shot();
        }
        
    }

    private void Shot()
    {
        //멀티게임일땐 사격 처리를 RPC를 통해서 한다.
        photonView.RPC("ShotProcessOnServer", RpcTarget.MasterClient); // 마스터 클라이언트만 Shot 실행, 나머지는 동기화 시킴
        if (this.magAmmo <= 0)
        {
            this.Reload(); // 탄창 내 총알이 0이 되면 장전 시작
            return;
        }
        else
        {
            this.magAmmo--; // 탄창 내 총알 수 감소
        }
        #region 싱글 게임일때 사격 처리 부분
        /*RaycastHit hit;
        Vector3 hitPos = Vector3.zero; // 총알이 맞은 위치 초기화
        if (Physics.Raycast(this.firePos.position, this.firePos.forward, out hit, this.fireDistance))
        {
            IDamageable target = hit.collider.GetComponent<IDamageable>();
            if (target != null)
            {
                
                target.OnDamage(gunData.damage, hit.point, hit.normal); // 맞은 대상에게 데미지 적용
            }
            hitPos = hit.point; // 맞은 위치를 저장
        }
        else
        {
            hitPos = this.firePos.position + (this.firePos.forward * this.fireDistance); // 맞은 위치가 없으면 사정거리 끝으로 설정
        }
        StartCoroutine(this.ShotEffect(hitPos)); // 총구 플래시와 라인 렌더러 이펙트 재생*/
        #endregion
    }
    [PunRPC]
    private void ShotProcessOnServer()
    {
        RaycastHit hit;
        Vector3 hitPos = Vector3.zero; // 총알이 맞은 위치 초기화

        if (Physics.Raycast(this.firePos.position, this.firePos.forward, out hit, this.fireDistance))
        {
            IDamageable target = hit.collider.GetComponent<IDamageable>();

            if(target != null)
            {
                target.OnDamage(gunData.damage, hit.point, hit.normal); // 맞은 대상에게 데미지 적용
            }

            hitPos = hit.point; // 맞은 위치를 저장
        }
        else
        {
            //안맞았다면
            hitPos = this.firePos.position + (this.firePos.forward * this.fireDistance); // 맞은 위치가 없으면 사정거리 끝으로 설정
        }

        //발사 이팩트 재생(모든 클라이언트에서)
        photonView.RPC("ShotEffectOnServer", RpcTarget.All, hitPos); // 모든 클라이언트에서 ShotEffectOnServer 실행
    }

    [PunRPC]
    private void ShotEffectOnServer(Vector3 hitPos)
    {
        StartCoroutine(this.ShotEffect(hitPos)); // 총구 플래시와 라인 렌더러 이펙트 재생
    }

    private WaitForSeconds shotEffectTime;
    IEnumerator ShotEffect(Vector3 hitPos)
    {
        this.muzzleFlashEffect.Play(); // 총구 플래시 이펙트 재생
        this.shellEffect.Play(); // 탄피 이펙트 재생
        this.source.PlayOneShot(gunData.shotClip); // 사격 사운드 재생

        this.lineRenderer.SetPosition(0, this.firePos.position); // 라인 렌더러의 시작점을 총구 위치로 설정
        this.lineRenderer.SetPosition(1, hitPos); 
        // 라인 렌더러의 끝점을 총구 위치에서 사정거리만큼 앞쪽으로 설정

        this.lineRenderer.enabled = true; // 라인 렌더러 활성화
        yield return this.shotEffectTime;
        this.lineRenderer.enabled = false; // 라인 렌더러 비활성화
    }
    public bool Reload()
    {
        if (this.State == eState.Reload || this.ammo <= 0 || this.magAmmo >= this.gunData.magCapacity)
            return false;// 이미 장전 중이거나, 남은 총알이 없거나, 탄창이 가득 찼을 때는 장전 불가

        StartCoroutine(ReloadRoutine()); // 장전 코루틴 시작
        return true; // 장전 시작 성공
    }
    private WaitForSeconds reloadTime; // 장전 시간
    IEnumerator ReloadRoutine()
    {
        this.State = eState.Reload; // 상태를 Reload로 변경
        this.reloadAnimationAction(); // 재장전 애니메이션 트리거 호출
        this.source.PlayOneShot(gunData.reloadClip); // 장전 사운드 재생
        yield return this.reloadTime; // 장전 시간만큼 대기
        int ammoToReload = Mathf.Min(gunData.magCapacity - this.magAmmo, ammo); // 최대 탄창 용량이 되기위해 필요한 총알 수 또는 남은 총알 수 중 작은 값
        magAmmo += ammoToReload; // 탄창 내 총알 수 증가
        ammo -= ammoToReload; // 남은 총알 수 감소
        this.State = eState.Ready; // 상태를 Ready로 변경
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext((int)this.State); // 현재 총 상태를 전송
            stream.SendNext(this.magAmmo); // 현재 탄창 내 총알 수 전송
            stream.SendNext(this.ammo); // 현재 보유 중인 총알 수 전송
        }
        else
        {
            this.State = (eState)stream.ReceiveNext(); // 총 상태를 수신
            this.magAmmo = (int)stream.ReceiveNext(); // 탄창 내 총알 수를 수신
            this.ammo = (int)stream.ReceiveNext(); // 보유 중인 총알 수를 수신
        }
    }

}
