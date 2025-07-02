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
//Scriptable : ���� �ٲܶ� �ٲ�°� �Ҹ��� �ٲ�°� �ƴ϶�, �װ� �� �ٲٷ��� �ʹ� �� �ڵ尡 �ʿ��ϴ�
// �׶� �ʿ��� ����.

public class TPSFire : MonoBehaviour
{
    //�ѽ��
    //1. �Ѿ� , 2. �߻� ��ġ 3. Input�� Fire
    //4. �� źâ�� ��, 5. ���� �ӵ�
    
    //�Ѿ�
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

    public int bulletClip; // źâ
    public int maxBulletClip = 40; // źâ �ִ�ġ
    [SerializeField] private Image bulletCntUIImage;
    [SerializeField] private Text bulletCntUIText;

    public float shotDelay = 0.1f; // ���簣 �ӵ�
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

        this.layerMask = 1 << this.enemyLayer | 1 << this.obstacleLayer;//��Ʈ ����

    }

    private void BulletChange(int cnt)
    {
        this.bulletClip = cnt;
        this.bulletCntUIText.text = $"{cnt} / {this.maxBulletClip}";
        this.bulletCntUIImage.fillAmount = (float)this.bulletClip / (float)this.maxBulletClip;
    }

    void Update()
    {
        Debug.DrawRay(firePos.position, firePos.forward * 20f, Color.white); // ���� �����°� üũ


        if (EventSystem.current.IsPointerOverGameObject()) return;
        //��ư�� ��Ҵٸ� ���� �ڵ� ���� , �̺�Ʈ ��

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



            /*// �Ѿ��� ��ġ�� �߻� ��ġ�� ����
            this.bulletPool[this.bulletPoolCnt].transform.position = this.firePos.position;

            // �Ѿ��� ȸ���� �߻� ��ġ�� ȸ���� �����ϰ� �����ϸ�, ���� ������ �״�� ����˴ϴ�.
            this.bulletPool[this.bulletPoolCnt].transform.rotation = this.firePos.rotation;

            this.bulletPool[this.bulletPoolCnt].SetActive(true);
            
            

            //Debug.Log($"{this.bulletPoolCnt}��° �Ѿ� �߻�");

            this.bulletPoolCnt++;
            if(this.bulletPoolCnt == this.bulletPool.Length)
            {
                this.bulletPoolCnt = 0;
                //Debug.Log("Ǯ ����");
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
