using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager Instance;
    [Header("������Ʈ Ǯ")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject e_BulletPrefab;
    [SerializeField] private int maxBulletPool = 15;
    [SerializeField] private List<GameObject> bulletPool = new List<GameObject>();
    [SerializeField] private List<GameObject> e_BulletPool = new List<GameObject>();


    int cnt = 0;
    int e_cnt = 0;
    GameObject bulletPoolObject;
    GameObject e_BulletPoolObject;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
        this.bulletPoolObject = new GameObject("Bullet Pool");
        this.e_BulletPoolObject = new GameObject("Enemy Bullet Pool");
        CreateBulletPool(this.bulletPoolObject,this.bulletPool, this.bulletPrefab, ref this.cnt);
        CreateBulletPool(this.e_BulletPoolObject,this.e_BulletPool,this.e_BulletPrefab, ref this.e_cnt);
    }

    private void CreateBulletPool(GameObject poolObject,List<GameObject> Pool, GameObject prefab, ref int cnt)
    {
        

        for ( ; cnt < this.maxBulletPool; )
        {
            CreateBullet(poolObject,Pool, prefab, ref cnt);

        }
    }

    private void CreateBullet(GameObject bulletPool, List<GameObject> Pool, GameObject prefab, ref int cnt)
    {
        Pool.Add(Instantiate(prefab, bulletPool.transform));
        Pool[cnt].name = $"{cnt + 1}��";
        Pool[cnt].SetActive(false);
        cnt++;
    }

    public GameObject GetBullet()
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {
            if (!bulletPool[i].activeSelf)
            {
                return bulletPool[i]; // for������ Ȱ��ȭ�� �ȵǾ��ִ°� ã�Ƽ� ã���� �¸� return���� �ѱ�
            }
        }
        //����� ���Դٴ°� ���� Ȱ��ȭ��°ǵ� �׷� ���⼭ ���� �����ؼ� list�� ����ְ�, �װ� ��ȯ
        CreateBullet(this.bulletPoolObject,this.bulletPool, this.bulletPrefab, ref cnt);
        return this.bulletPool[cnt - 1];
    }
    public GameObject GetE_Bullet()
    {
        for (int i = 0; i < e_BulletPool.Count; i++)
        {
            if (!e_BulletPool[i].activeSelf)
            {
                return e_BulletPool[i]; // for������ Ȱ��ȭ�� �ȵǾ��ִ°� ã�Ƽ� ã���� �¸� return���� �ѱ�
            }
        }
        //����� ���Դٴ°� ���� Ȱ��ȭ��°ǵ� �׷� ���⼭ ���� �����ؼ� list�� ����ְ�, �װ� ��ȯ
        CreateBullet(this.e_BulletPoolObject, this.e_BulletPool, this.e_BulletPrefab, ref e_cnt);
        return this.e_BulletPool[e_cnt - 1];
    }

}
