using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] items; // 생성할 아이템
    public Transform playerTr; // 플레이어 트랜스폼

    public float maxDistance = 5f; // 아이템이 플레이어로부터 떨어질 최대 거리

    public float timeBetSpawnMax = 7f; // 아이템 생성 간격 최대 시간
    public float timeBetSpawnMin = 2f; // 아이템 생성 간격 최소 시간
    private float timeBetSpawn; // 아이템 생성 간격 시간

    private float lastSpawnTime; // 마지막 아이템 생성 시간
    void Start()
    {
        this.playerTr = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어 트랜스폼 찾기
        this.timeBetSpawn = Random.Range(this.timeBetSpawnMin, this.timeBetSpawnMax); // 아이템 생성 간격 시간 설정
        this.lastSpawnTime = 0; // 마지막 아이템 생성 시간 초기화
    }
    void Update()
    {
        if(Time.time >= lastSpawnTime + timeBetSpawn && this.playerTr != null)
        {
            this.lastSpawnTime = Time.time; // 마지막 아이템 생성 시간 갱신
            this.timeBetSpawn = Random.Range(this.timeBetSpawnMin, this.timeBetSpawnMax); // 다음 아이템 생성 간격 시간 설정
            Spawn();
        }
    }

    private void Spawn()
    {
        Vector3 spawnPos = GetRandomPointOnNavMesh(this.playerTr.position, this.maxDistance);
        spawnPos += Vector3.up * 0.5f; // 아이템이 땅에 닿도록 약간 위로 이동

        var itemSelected = items[Random.Range(0, items.Length)]; // 아이템 목록에서 랜덤으로 선택
        GameObject item = Instantiate(itemSelected, spawnPos, Quaternion.identity); // 아이템 생성

        Destroy(item, 5f); // 5초 후 아이템 제거
    }

    private Vector3 GetRandomPointOnNavMesh(Vector3 centor, float dist)
    {
        Vector3 randomPos = Random.insideUnitSphere * dist + centor; // 랜덤한 위치 생성
        NavMeshHit hit; // 네비메쉬 히트 정보
        NavMesh.SamplePosition(randomPos, out hit, dist, NavMesh.AllAreas); // 네비메쉬에서 위치 샘플링
        return hit.position;
    }
}
