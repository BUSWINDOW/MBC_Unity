using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] items; // ������ ������
    public Transform playerTr; // �÷��̾� Ʈ������

    public float maxDistance = 5f; // �������� �÷��̾�κ��� ������ �ִ� �Ÿ�

    public float timeBetSpawnMax = 7f; // ������ ���� ���� �ִ� �ð�
    public float timeBetSpawnMin = 2f; // ������ ���� ���� �ּ� �ð�
    private float timeBetSpawn; // ������ ���� ���� �ð�

    private float lastSpawnTime; // ������ ������ ���� �ð�
    void Start()
    {
        this.playerTr = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾� Ʈ������ ã��
        this.timeBetSpawn = Random.Range(this.timeBetSpawnMin, this.timeBetSpawnMax); // ������ ���� ���� �ð� ����
        this.lastSpawnTime = 0; // ������ ������ ���� �ð� �ʱ�ȭ
    }
    void Update()
    {
        if(Time.time >= lastSpawnTime + timeBetSpawn && this.playerTr != null)
        {
            this.lastSpawnTime = Time.time; // ������ ������ ���� �ð� ����
            this.timeBetSpawn = Random.Range(this.timeBetSpawnMin, this.timeBetSpawnMax); // ���� ������ ���� ���� �ð� ����
            Spawn();
        }
    }

    private void Spawn()
    {
        Vector3 spawnPos = GetRandomPointOnNavMesh(this.playerTr.position, this.maxDistance);
        spawnPos += Vector3.up * 0.5f; // �������� ���� �굵�� �ణ ���� �̵�

        var itemSelected = items[Random.Range(0, items.Length)]; // ������ ��Ͽ��� �������� ����
        GameObject item = Instantiate(itemSelected, spawnPos, Quaternion.identity); // ������ ����

        Destroy(item, 5f); // 5�� �� ������ ����
    }

    private Vector3 GetRandomPointOnNavMesh(Vector3 centor, float dist)
    {
        Vector3 randomPos = Random.insideUnitSphere * dist + centor; // ������ ��ġ ����
        NavMeshHit hit; // �׺�޽� ��Ʈ ����
        NavMesh.SamplePosition(randomPos, out hit, dist, NavMesh.AllAreas); // �׺�޽����� ��ġ ���ø�
        return hit.position;
    }
}
