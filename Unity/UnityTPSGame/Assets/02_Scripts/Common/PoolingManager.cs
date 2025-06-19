using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager Instance;
    [Header("������Ʈ Ǯ")]
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
        this.bulletPool[cnt].name = $"{cnt + 1}��";
        this.bulletPool[cnt].SetActive(false);
        this.cnt++;
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
        CreateBullet(this.bulletPoolObject, cnt);
        return this.bulletPool[cnt - 1];
    }

}
