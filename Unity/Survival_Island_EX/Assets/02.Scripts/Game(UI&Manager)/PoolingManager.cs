using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager instance;
    private GameObject bulletPoolObject;

    [SerializeField]
    private GameObject bulletPrefab;
    private List<GameObject> bulletPool;
    private int cnt = 0;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
        this.bulletPoolObject = new GameObject("bulletPool");
        this.bulletPool = new List<GameObject>();
        for(; cnt < 10;)
        {
            CreateBullet();
        }
    }

    private void CreateBullet()
    {
        this.bulletPool.Add(Instantiate(bulletPrefab, this.bulletPoolObject.transform));
        this.bulletPool[cnt].name = $"{cnt + 1}발";
        this.bulletPool[cnt].SetActive(false);
        cnt++;
    }
    public GameObject GetBullet()
    {
        for(int i = 0; i< this.bulletPool.Count; i++)
        {
            if (!this.bulletPool[i].activeSelf)
            {
                return this.bulletPool[i]; // 활성화 안된거 반환
            }
        }
        //전부 활성화라면 새로 생성 후 그거 반환
        CreateBullet();
        return this.bulletPool[cnt - 1];
    }
}
