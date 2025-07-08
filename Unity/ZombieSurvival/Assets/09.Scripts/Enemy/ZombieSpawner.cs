using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;

    public ZombieData[] zombieDatas; // 좀비 데이터 배열
    public Transform[] spawnPoints; // 좀비가 생성될 위치

    private List<Zombie> zombiePool = new List<Zombie>(); // 생성된 좀비 풀링
    private List<Zombie> zombies = new List<Zombie>(); // 생성된 좀비 목록

    private int wave; // 현재 웨이브

    private void Start()
    {
        this.zombieDatas = Resources.LoadAll<ZombieData>("Scriptable"); // Resources 폴더에서 좀비 데이터 로드
    }
    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        if (zombies.Count <= 0)
        {
            SpawnWave();
        }
        
    }

    private void UpdateUI()
    {
        if (GameManager.Instance != null && UIManager.Instance != null)
        {
            UIManager.Instance.UpdateWaveTxt(wave,this.zombies.Count);
        }
    }

    private void SpawnWave() // 웨이브 시작
    {
        wave++;
        int zombieCount = Mathf.RoundToInt(wave * 1.5f); // 웨이브마다 좀비 수 증가
        for (int i = 0; i < zombieCount; i++)
        {
            CreateZombie();
        }
        UpdateUI(); // UI 업데이트
    }

    private void CreateZombie()
    {
        
        ZombieData data = zombieDatas[UnityEngine.Random.Range(0, zombieDatas.Length)]; // 랜덤으로 좀비 데이터 선택
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)]; // 랜덤으로 생성 위치 선택

        for(int i = 0; i<this.zombiePool.Count; i++)
        {
            if (!zombiePool[i].gameObject.activeSelf)
            {
                zombiePool[i].transform.position = spawnPoint.position; // 생성 위치로 이동
                zombiePool[i].Setup(data); // 좀비 데이터 설정
                this.zombies.Add(zombiePool[i]); // 생성된 좀비 목록에 추가
                zombiePool[i].gameObject.SetActive(true); // 비활성화된 좀비가 있다면 활성화
                
                return;
            }
        }
        //여기로 왔으면 모든게 활성화 되어있거나, 생성된게 없거나
        Zombie zombie = Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity).GetComponent<Zombie>(); // 좀비 생성
        zombie.Setup(data); // 좀비 데이터 설정
        zombie.DieAction += () =>
        {
            this.zombies.Remove(zombie); // 좀비가 죽으면 목록에서 제거
                                         //목록에서만 제거


            UpdateUI(); // UI 업데이트
            GameManager.Instance.AddScore(100); // 점수 업데이트
        };
        this.zombiePool.Add(zombie); // 생성된 좀비 풀링에 추가
        this.zombies.Add(zombie); // 생성된 좀비 목록에 추가

    }
}
