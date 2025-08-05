using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEditor;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject playerPrefab;

    List<GameObject> enemyPool;
    List<GameObject> playerPool;

    GameObject playerParentForFind; // 플레이어가 꺼져있을때도 적이 플레이어를 target에 넣기위한 부모 오브젝트
    public void Init()
    {
        //start에서 풀 생성
        this.enemyPool = new List<GameObject>();
        this.playerPool = new List<GameObject>();
        this.playerParentForFind = new GameObject("PlayerParent");
    }
    public GameObject GetEnemy()
    {
        GameManager.Instance.aliveEnemyCnt++;
        //풀에서 적 가져오기
        foreach (GameObject enemy in this.enemyPool)
        {
            if (!enemy.activeSelf)
            {
                enemy.SetActive(true);
                return enemy;
            }
        }
        //여기로 나왔다는건 모든 풀이 활성화
        //새로 생성 후 그걸 반환
        CreateObject(this.enemyPool, this.enemyPrefab,()=>{
            GameManager.Instance.aliveEnemyCnt--;
            GameManager.Instance.gameData.Kill++;
            EditorUtility.SetDirty(GameManager.Instance.gameData);
        });
        this.enemyPool[enemyPool.Count - 1].SetActive(true);
        return this.enemyPool[enemyPool.Count-1];
    }
    public GameObject GetPlayer()
    {
        foreach (GameObject player in this.playerPool)
        {
            if (!player.activeSelf) // 플레이어로 풀링하라 하셔서 이렇게 해놨는데,
                                    // 이러면 플레이어가 죽어있을때 새 플레이어가 생성되면 에러날거 같습니다.
                                    // 네트워크 기반이라면 내부에 컨트롤하는 플레이어가 있을때 true인 bool값을 하나 만들고, disConnect될 때 false가 되게하면
                                    // 풀링되는 플레이어를 만들 수 있을거같은데, 포톤을 붙이라는 말은 안하셨으니 생략하겠습니다.
            {
                player.SetActive(true);
                return player;
            }
        }
        //여기로 나왔다는건 모든 풀이 활성화
        //새로 생성 후 그걸 반환
        CreateObject(this.playerPool, this.playerPrefab,() =>
        {
            StartCoroutine(UtilScripts.WaitForSec(() =>
            {
                this.playerPool[playerPool.Count - 1].SetActive(true);
            }, 3));  // 죽으면 3초뒤 부활
        });
        this.playerPool[playerPool.Count - 1].SetActive(true);
        this.playerPool[playerPool.Count - 1].transform.SetParent(this.playerParentForFind.transform);
        return this.playerPool[playerPool.Count - 1];
    }
    public void CreateObject(List<GameObject> pool, GameObject prefab,Action dieAct)
    {
        var obj = Instantiate(prefab);
        obj.GetComponent<CombatTarget>().dieAction = dieAct;
        obj.SetActive(false);
        pool.Add(obj);
        //풀에 오브젝트 생성하기
    }
}
