using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject skelPrefab;
    public Transform spawnPos;
    int spawnCnt;
    void Start()
    {
        this.spawnCnt = 0;
        StartCoroutine(this.SpawnRoutine());
    }
    IEnumerator SpawnRoutine()
    {
        while(this.spawnCnt < 5)
        {
            var skel = Instantiate(skelPrefab,this.spawnPos.position,Quaternion.identity);
            this.spawnCnt++;
            yield return new WaitForSeconds(3);
        }
        yield return null;
    }
}
