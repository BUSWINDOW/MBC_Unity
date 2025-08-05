using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameData gameData;
    PoolingManager poolingManager;
    public int aliveEnemyCnt = 0;
    void Awake()
    {
        Instance = this;
        this.poolingManager = GameObject.Find("PoolingManager").GetComponent<PoolingManager>();
        this.poolingManager.Init();
    }
    private void Start()
    {
        this.poolingManager.GetPlayer();
        StartCoroutine(this.SpawnRoutine());
        
    }
    WaitForSeconds wsForSpawn = new WaitForSeconds(3);
    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return wsForSpawn;
            if (this.aliveEnemyCnt < 5)
            {
                this.poolingManager.GetEnemy();
            }
        }
    }

}
