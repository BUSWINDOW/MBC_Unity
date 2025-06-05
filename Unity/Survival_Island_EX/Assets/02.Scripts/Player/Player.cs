using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;


public class Player : MonoBehaviour
{
    private int bulletCnt;
    PlayerAnimationCtrl animationCtrl;
    PlayerSoundCtrl shootingSoundCtrl;
    PlayerShootEffectCtrl shootEffectCtrl;
    private static string fire = "Fire1";

    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform firePos;

    bool isReloading;
    bool isShooting;
    bool isRunning;
    void Start()
    {
        this.bulletCnt = 10;
        this.isReloading = false;
        this.isShooting = false;
        this.isRunning = false;
        this.GetComponent<FirstPersonController>().m_IsWalking = true;
        this.animationCtrl = GetComponent<PlayerAnimationCtrl>();
        this.shootingSoundCtrl = GetComponent<PlayerSoundCtrl>();
        this.shootEffectCtrl = GetComponent<PlayerShootEffectCtrl>();
    }

    void Update()
    {
        PlayerShoot();
        PlayerReloading();
        PlayerMove();
    }
    private void PlayerShoot()
    {
        if (Input.GetButtonDown(fire))
        {
            if (!isReloading&&!isShooting&&!isRunning)
            {
                if (this.bulletCnt == 0) //장전 부분
                {
                    Debug.Log("재장전 시작");
                    StartCoroutine(this.ReloadingRoutine());
                }
                else //사격 부분
                {
                    StartCoroutine(this.ShootingRoutine());
                    Debug.Log($"남은 총알 : {this.bulletCnt}");
                }
            }
            
        }
    }
    private void PlayerReloading()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isReloading && !isShooting)
            {
                Debug.Log("재장전 시작");
                StartCoroutine(this.ReloadingRoutine());
            }
        }
    }
    private void PlayerMove()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)&&!isReloading&&!isShooting)
        {
            this.animationCtrl.PlayerRunning();
            this.GetComponent<FirstPersonController>().m_IsWalking = false;
            this.isRunning = true;
            
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && !isReloading && !isShooting)
        {
            this.animationCtrl.PlayerStop();
            this.GetComponent<FirstPersonController>().m_IsWalking = true;
            this.isRunning = false;
        }
    }
    IEnumerator ReloadingRoutine()
    {
        this.isReloading=true;
        this.animationCtrl.PlayerReloading();
        this.shootingSoundCtrl.PlaySound(this.shootingSoundCtrl.reloadSound);
        yield return new WaitForSeconds(1);
        this.bulletCnt = 10;
        this.isReloading=false;
        Debug.Log("재장전 끝");
        this.animationCtrl.PlayerStop();
    }
    IEnumerator ShootingRoutine()
    {
        this.isShooting=true;
        this.bulletCnt--;
        this.animationCtrl.PlayerShoot();
        this.shootingSoundCtrl.PlaySound(this.shootingSoundCtrl.shootSound);
        this.shootEffectCtrl.PlayEffect();
        var bullet = Instantiate(this.bulletPrefab, this.firePos.position, this.firePos.rotation);
        yield return new WaitForSeconds(0.5f);
        this.isShooting = false;
        this.animationCtrl.PlayerStop();
    }
}
