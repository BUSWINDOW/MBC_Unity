using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using static UnityEditor.Progress;


public class Player : MonoBehaviour
{
    private int bulletCnt;
    public int m4BulletCnt;
    PlayerAnimationCtrl animationCtrl;
    PlayerSoundCtrl shootingSoundCtrl;
    PlayerShootEffectCtrl shootEffectCtrl;
    PlayerDamage damage;
    WeaponChange weaponChange;
    private static string fire = "Fire1";
    private static string skelTag = "Skel";

    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform firePos;

    public bool isReloading;
    public bool isShooting;
    public bool isRunning;
    public bool isContinueShooting;

    public int hp;
    public int maxhp;
    public int speed;
    public int granade;
    public int B_damage;
    //public ItemData[] equips = new ItemData[4];
    public Drop[] slots;

    //public GameData gameData;
    public GameDataSO gameData;

    public ItemInfo[] items;

    public static Action dieAction;

    void Start()
    {
        this.bulletCnt = 10;
        this.m4BulletCnt = 30;




        this.InitStats();



        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].itemEquipAction += (idx) =>
            {
                this.gameData.items[idx] = slots[idx].item;
                this.StatUpdate(this.slots[idx].item ,(CharValue, ItemValue) =>
                {
                    return CharValue + ItemValue;
                });
                UnityEditor.EditorUtility.SetDirty(this.gameData);
            };
            slots[i].itemRemoveAction += (idx) =>
            {
                this.StatUpdate(this.slots[idx].item, (CharValue, ItemValue) =>
                {
                    return CharValue - ItemValue;
                });
                this.gameData.items[idx] = null;
                UnityEditor.EditorUtility.SetDirty(this.gameData);
            };
            if (gameData.items[i].Idx != ItemData.eItemIdx.None)
            {
                Debug.Log(this.gameData.items[i].Idx);
                this.slots[i].item = this.gameData.items[i];
            }
        }

        this.hp = this.maxhp;


        
        InitSetting();
    }

    private void InitSetting()
    {
        this.isReloading = false;
        this.isShooting = false;
        this.isRunning = false;
        this.GetComponent<FirstPersonController>().m_IsWalking = true;
        this.animationCtrl = GetComponent<PlayerAnimationCtrl>();
        this.shootingSoundCtrl = GetComponent<PlayerSoundCtrl>();
        this.shootEffectCtrl = GetComponent<PlayerShootEffectCtrl>();
        this.weaponChange = GetComponent<WeaponChange>();
        this.damage = GetComponent<PlayerDamage>();
        this.damage.hitAction += () =>
        {
            this.hp -= 2;
            if (this.hp <= 0)
            {
                GameManager.instance.isGameOver = true;
                dieAction();

            }
        };
    }


    private void StatUpdate(ItemData item, Func<int,int,int> op) // 아이템이 갱신됐을때 스탯 적용용 함수
    {
        switch (item.Idx)
        {
            case ItemData.eItemIdx.None:
                break;
            case ItemData.eItemIdx.Hp:
                this.maxhp =op(this.maxhp, item.Value);
                break;
            case ItemData.eItemIdx.Speed:
                this.speed =op(this.speed, item.Value);
                break;
            case ItemData.eItemIdx.Shock:
                this.B_damage = op(this.B_damage, item.Value);
                break;
            case ItemData.eItemIdx.Granade:
                this.granade = op(this.granade, item.Value);
                break;
        }

    }

    private void InitStats()
    {
        this.maxhp = gameData.hp;
        this.speed = gameData.speed;
        this.B_damage = gameData.damage;
        this.granade = gameData.granade;
        for (int i = 0; i < gameData.items.Length; i++) 
        {
            if (gameData.items[i].Idx != ItemData.eItemIdx.None)
                this.items[(int)gameData.items[i].Idx - 1].transform.SetParent(this.slots[i].transform);
            switch (gameData.items[i].Idx)
            {
                case ItemData.eItemIdx.None:
                    break;
                case ItemData.eItemIdx.Hp:
                    this.maxhp += gameData.items[i].Value;
                    
                    break;
                case ItemData.eItemIdx.Speed:
                    this.speed += gameData.items[i].Value;
                    break;
                case ItemData.eItemIdx.Granade:
                    this.granade += gameData.items[i].Value;
                    break;
                case ItemData.eItemIdx.Shock:
                    this.B_damage += gameData.items[i].Value;
                    break;
            }
        }
    }





    void Update()
    {
        PlayerShoot();
        PlayerReloading();
        PlayerMove();

        //rayCast로 오토 슈팅
        RaycastHit hit;
        if(Physics.Raycast(this.firePos.position, this.firePos.forward, out hit, 20f, 1<<6 | 1<< 7))
        {
            if (hit.collider.CompareTag(skelTag))
            {
                if (!isReloading && !isShooting && !isRunning)
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

        

    }
    private void PlayerShoot()
    {
        if (Input.GetButtonDown(fire)&&!this.weaponChange.isHaveM4a1)
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
        else if (Input.GetButtonDown(fire) && this.weaponChange.isHaveM4a1)
        {
            this.isContinueShooting = true;
            StartCoroutine(this.ContinueShootingRoutine());
        }
        else if(Input.GetButtonUp(fire) && this.weaponChange.isHaveM4a1)
        {
            this.isContinueShooting = false;
            this.isShooting = false ;
            StopAllCoroutines();
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
        //var bullet = Instantiate(this.bulletPrefab, this.firePos.position, this.firePos.rotation);
        var bullet = PoolingManager.instance.GetBullet();
        bullet.transform.position = this.firePos.position;
        bullet.transform.rotation = this.firePos.rotation;
        bullet.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        this.isShooting = false;
        this.animationCtrl.PlayerStop();
    }
    IEnumerator ContinueShootingRoutine()
    {
        while (isContinueShooting) 
        {
            while(m4BulletCnt > 0)
            {
                this.isShooting = true;
                this.m4BulletCnt--;
                this.animationCtrl.PlayerShoot();
                this.shootingSoundCtrl.PlaySound(this.shootingSoundCtrl.shootSound);
                this.shootEffectCtrl.PlayEffect();
                var bullet = Instantiate(this.bulletPrefab, this.firePos.position, this.firePos.rotation);
                yield return new WaitForSeconds(0.1f);
                this.isShooting = false;
                this.animationCtrl.PlayerStop();
            }
            this.isReloading = true;
            this.animationCtrl.PlayerReloading();
            this.shootingSoundCtrl.PlaySound(this.shootingSoundCtrl.reloadSound);
            yield return new WaitForSeconds(1);
            this.m4BulletCnt = 30;
            this.isReloading = false;
            Debug.Log("재장전 끝");
            this.animationCtrl.PlayerStop();
            yield return null;
        }
        
    }
}
