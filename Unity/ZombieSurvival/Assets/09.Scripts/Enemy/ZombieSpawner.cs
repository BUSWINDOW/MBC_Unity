using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;

    public ZombieData[] zombieDatas; // ���� ������ �迭
    public Transform[] spawnPoints; // ���� ������ ��ġ

    private List<Zombie> zombiePool = new List<Zombie>(); // ������ ���� Ǯ��
    private List<Zombie> zombies = new List<Zombie>(); // ������ ���� ���

    private int wave; // ���� ���̺�

    private void Start()
    {
        this.zombieDatas = Resources.LoadAll<ZombieData>("Scriptable"); // Resources �������� ���� ������ �ε�
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

    private void SpawnWave() // ���̺� ����
    {
        wave++;
        int zombieCount = Mathf.RoundToInt(wave * 1.5f); // ���̺긶�� ���� �� ����
        for (int i = 0; i < zombieCount; i++)
        {
            CreateZombie();
        }
        UpdateUI(); // UI ������Ʈ
    }

    private void CreateZombie()
    {
        
        ZombieData data = zombieDatas[UnityEngine.Random.Range(0, zombieDatas.Length)]; // �������� ���� ������ ����
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)]; // �������� ���� ��ġ ����

        for(int i = 0; i<this.zombiePool.Count; i++)
        {
            if (!zombiePool[i].gameObject.activeSelf)
            {
                zombiePool[i].transform.position = spawnPoint.position; // ���� ��ġ�� �̵�
                zombiePool[i].Setup(data); // ���� ������ ����
                this.zombies.Add(zombiePool[i]); // ������ ���� ��Ͽ� �߰�
                zombiePool[i].gameObject.SetActive(true); // ��Ȱ��ȭ�� ���� �ִٸ� Ȱ��ȭ
                
                return;
            }
        }
        //����� ������ ���� Ȱ��ȭ �Ǿ��ְų�, �����Ȱ� ���ų�
        Zombie zombie = Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity).GetComponent<Zombie>(); // ���� ����
        zombie.Setup(data); // ���� ������ ����
        zombie.DieAction += () =>
        {
            this.zombies.Remove(zombie); // ���� ������ ��Ͽ��� ����
                                         //��Ͽ����� ����


            UpdateUI(); // UI ������Ʈ
            GameManager.Instance.AddScore(100); // ���� ������Ʈ
        };
        this.zombiePool.Add(zombie); // ������ ���� Ǯ���� �߰�
        this.zombies.Add(zombie); // ������ ���� ��Ͽ� �߰�

    }
}
