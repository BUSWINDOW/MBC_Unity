using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager Instance;
    [Header("오브젝트 풀")]
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
        Pool[cnt].name = $"{cnt + 1}발";
        Pool[cnt].SetActive(false);
        cnt++;
    }

    public GameObject GetBullet()
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {
            if (!bulletPool[i].activeSelf)
            {
                return bulletPool[i]; // for문으로 활성화가 안되어있는걸 찾아서 찾으면 걔를 return으로 넘김
            }
        }
        //여기로 나왔다는건 모든게 활성화라는건데 그럼 여기서 새로 생성해서 list에 집어넣고, 그걸 반환
        CreateBullet(this.bulletPoolObject,this.bulletPool, this.bulletPrefab, ref cnt);
        return this.bulletPool[cnt - 1];
    }
    public GameObject GetE_Bullet()
    {
        for (int i = 0; i < e_BulletPool.Count; i++)
        {
            if (!e_BulletPool[i].activeSelf)
            {
                return e_BulletPool[i]; // for문으로 활성화가 안되어있는걸 찾아서 찾으면 걔를 return으로 넘김
            }
        }
        //여기로 나왔다는건 모든게 활성화라는건데 그럼 여기서 새로 생성해서 list에 집어넣고, 그걸 반환
        CreateBullet(this.e_BulletPoolObject, this.e_BulletPool, this.e_BulletPrefab, ref e_cnt);
        return this.e_BulletPool[e_cnt - 1];
    }

}
