using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
[System.Serializable]
public struct PlayerSfx
{
    public AudioClip[] fire;
    public AudioClip[] reload;
}
//Scriptable : 총을 바꿀때 바뀌는건 소리만 바뀌는게 아니라서, 그걸 다 바꾸려면 너무 긴 코드가 필요하다
// 그때 필요한 내용.

public class TPSFire : MonoBehaviour
{
    //총쏘기
    //1. 총알 , 2. 발사 위치 3. Input의 Fire
    //4. 한 탄창의 양, 5. 연사 속도
    
    //총알
    /*public GameObject bulletPrefab;
    public GameObject[] bulletPool;
    private int bulletPoolCnt;
    public int maxBulletPool = 150;
*/

    public enum eWeaponType
    {
        Rifle, Shotgun
    }
    public eWeaponType curWeaponType = eWeaponType.Rifle;
    public PlayerSfx playerSfx;

    public Transform firePos;
    
    public TPSPlayerInput input;

    public int bulletClip; // 탄창
    public int maxBulletClip = 40; // 탄창 최대치
    [SerializeField] private Image bulletCntUIImage;
    [SerializeField] private Text bulletCntUIText;

    public float shotDelay = 0.1f; // 연사간 속도
    private float prevTime;

    bool isReload;

    public ParticleSystem cartiage;
    public ParticleSystem muzzleFlash;

    private AudioSource source;
    private Animator anim;

    public Sprite[] weaponIcons;
    public Image weaponImageUI;

    public PlayerRifleData RifleGunData;
    public PlayerRifleData ShotGunData;

    private PlayerRifleData currentGun;

    private int enemyLayer;
    private int obstacleLayer;

    private int layerMask;

    private readonly static string enemyTag = "Enemy";

    private bool isFire = false;
    private float prevFire;
    public float autiFireRate = 0.15f;
    void Start()
    {
        this.prevTime = Time.time;
        this.isReload = false;
        /*this.bulletPool = new GameObject[this.maxBulletPool];
        for (int i = 0; i < bulletPool.Length; i++)
        {
            this.bulletPool[i] = Instantiate(bulletPrefab);
        }
        this.bulletPoolCnt = 0;*/

        this.input = GetComponent<TPSPlayerInput>();
        this.source = GetComponent<AudioSource>();
        this.anim = GetComponent<Animator>();

        BulletChange(this.maxBulletClip);
        this.currentGun = this.RifleGunData;

        this.enemyLayer = LayerMask.NameToLayer("Enemy");
        obstacleLayer = LayerMask.NameToLayer("Object");

        this.layerMask = 1 << this.enemyLayer | 1 << this.obstacleLayer;//비트 연산

    }

    private void BulletChange(int cnt)
    {
        this.bulletClip = cnt;
        this.bulletCntUIText.text = $"{cnt} / {this.maxBulletClip}";
        this.bulletCntUIImage.fillAmount = (float)this.bulletClip / (float)this.maxBulletClip;
    }

    void Update()
    {
        Debug.DrawRay(firePos.position, firePos.forward * 20f, Color.white); // 레이 나가는거 체크


        if (EventSystem.current.IsPointerOverGameObject()) return;
        //버튼에 닿았다면 하위 코드 생략 , 이벤트 훅

        /*RaycastHit hit;
        if (Physics.Raycast(this.firePos.position, firePos.forward, out hit, 20f, this.layerMask))
        {
            isFire = (hit.collider.CompareTag(enemyTag));
        }
        else
            isFire = false;

        if (!isReload && isFire)
        {
            if (Time.time > this.prevFire)
                this.Shot();
            this.prevFire = Time.time + this.autiFireRate;
        }
*/

        if (this.input.Fire && (Time.time - this.prevTime > this.shotDelay) && !this.input.isRun && !this.isReload)
        {
            this.prevTime = Time.time;



            /*// 총알의 위치를 발사 위치로 설정
            this.bulletPool[this.bulletPoolCnt].transform.position = this.firePos.position;

            // 총알의 회전을 발사 위치의 회전과 동일하게 설정하면, 정면 방향이 그대로 적용됩니다.
            this.bulletPool[this.bulletPoolCnt].transform.rotation = this.firePos.rotation;

            this.bulletPool[this.bulletPoolCnt].SetActive(true);
            
            

            //Debug.Log($"{this.bulletPoolCnt}번째 총알 발사");

            this.bulletPoolCnt++;
            if(this.bulletPoolCnt == this.bulletPool.Length)
            {
                this.bulletPoolCnt = 0;
                //Debug.Log("풀 원점");
            }*/

            Shot();
            if (this.bulletClip == 0)
            {
                this.Reload();
            }

        }

    }

    private void Shot()
    {
        var bullet = PoolingManager.Instance.GetBullet();
        this.BulletChange(--this.bulletClip);
        bullet.transform.position = this.firePos.position;
        bullet.transform.rotation = this.firePos.rotation;
        bullet.SetActive(true);
        this.source.PlayOneShot(this.currentGun.shotClip);

        this.muzzleFlash.Play();
        this.cartiage.Play();
    }
    void Reload()
    {
        this.isReload=true;
        this.input.Reload = true;
        this.source.PlayOneShot(this.currentGun.reloadClip);
        StartCoroutine(this.WaitSomeSec(() =>
        {
            this.BulletChange(this.maxBulletClip);
            this.isReload = false;
            this.input.Reload = false;
            //Debug.Log(this.anim.GetCurrentAnimatorClipInfo(1).Length);
            //Debug.Log(this.anim.GetCurrentAnimatorClipInfo(1));
        }, 1.5f/*this.anim.GetCurrentAnimatorClipInfo(1).Length*/));
    }
    IEnumerator WaitSomeSec(Action act, float sec)
    {
        yield return new WaitForSeconds(sec);
        act();
    }

    public void OnChangeWeapon()
    {
        this.curWeaponType = (eWeaponType)((int)++this.curWeaponType % 2);
        weaponImageUI.sprite = this.weaponIcons[(int)this.curWeaponType];
        this.currentGun = this.currentGun == this.RifleGunData ? this.ShotGunData : this.RifleGunData;
    }
}
