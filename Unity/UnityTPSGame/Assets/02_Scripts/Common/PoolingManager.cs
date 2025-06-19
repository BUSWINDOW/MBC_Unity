using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager Instance;
    [Header("오브젝트 풀")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int maxBulletPool = 150;
    [SerializeField] private List<GameObject> bulletPool = new List<GameObject>();

    int cnt = 0;
    GameObject bulletPoolObject;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
        this.bulletPoolObject = new GameObject("Bullet Pool");
        CreateBulletPool();
    }

    private void CreateBulletPool()
    {
        

        for ( ; cnt < this.maxBulletPool; )
        {
            CreateBullet(this.bulletPoolObject , cnt);

        }
    }

    private void CreateBullet(GameObject bulletPool, int cnt)
    {
        this.bulletPool.Add(Instantiate(bulletPrefab, bulletPool.transform));
        this.bulletPool[cnt].name = $"{cnt + 1}발";
        this.bulletPool[cnt].SetActive(false);
        this.cnt++;
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
        CreateBullet(this.bulletPoolObject, cnt);
        return this.bulletPool[cnt - 1];
    }

}
