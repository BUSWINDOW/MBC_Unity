using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Transform firePos;
    public TPSPlayerInput input;

    public int bulletClip; // źâ
    public int maxBulletClip = 40; // źâ �ִ�ġ

    public float shotDelay = 0.3f; // ���簣 �ӵ�
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

            var bullet = PoolingManager.Instance.GetBullet();
            bullet.transform.position = this.firePos.position;
            bullet.transform.rotation = this.firePos.rotation;
            bullet.SetActive(true);
            this.muzzleFlash.Play();
            this.cartiage.Play();

        }

    }
}
