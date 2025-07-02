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



    //적
    public GameObject skelPrefab;
    public Transform spawnPos;
    int MaxSpawnCnt = 5;
    private GameObject EnemyPoolObject;
    private List<GameObject> enemyPool;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        this.bulletPoolObject = Instantiate(new GameObject("bulletPool"), this.transform);
        this.bulletPool = new List<GameObject>();
        for(; cnt < 10;)
        {
            CreateBullet();
        }

        this.EnemyPoolObject = Instantiate(new GameObject("Enemy Pool"), this.transform);
        this.enemyPool = new List<GameObject>();
        for(int i = 0; i< this.MaxSpawnCnt; i++)
        {
            this.enemyPool.Add(Instantiate(skelPrefab, this.EnemyPoolObject.transform));
            this.enemyPool[i].SetActive(false);
        }
        StartCoroutine(this.SpawnRoutine());
    }


    IEnumerator SpawnRoutine()
    {
        while (!GameManager.instance.isGameOver)
        {
            SpawnSkel();
            yield return new WaitForSeconds(3);
        }
    }

    private void SpawnSkel()
    {
        for(int i = 0;i< this.MaxSpawnCnt; i++)
        {
            if (!this.enemyPool[i].activeSelf)
            {
                this.enemyPool[i].transform.position = this.spawnPos.position;
                this.enemyPool[i].SetActive(true);
                return;
            }
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
