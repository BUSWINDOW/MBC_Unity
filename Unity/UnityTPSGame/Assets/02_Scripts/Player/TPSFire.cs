using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Transform firePos;
    public TPSPlayerInput input;

    public int bulletClip; // 탄창
    public int maxBulletClip = 40; // 탄창 최대치

    public float shotDelay = 0.3f; // 연사간 속도
    private float prevTime;


    public ParticleSystem cartiage;
    public ParticleSystem muzzleFlash;


    void Start()
    {
        this.prevTime = Time.time;

        /*this.bulletPool = new GameObject[this.maxBulletPool];
        for (int i = 0; i < bulletPool.Length; i++)
        {
            this.bulletPool[i] = Instantiate(bulletPrefab);
        }
        this.bulletPoolCnt = 0;*/

        this.input = GetComponent<TPSPlayerInput>();

        this.bulletClip = this.maxBulletClip;
    }

    void Update()
    {
        if (this.input.Fire && (Time.time - this.prevTime > this.shotDelay) && !this.input.isRun)
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

            var bullet = PoolingManager.Instance.GetBullet();
            bullet.transform.position = this.firePos.position;
            bullet.transform.rotation = this.firePos.rotation;
            bullet.SetActive(true);
            this.muzzleFlash.Play();
            this.cartiage.Play();

        }

    }
}
