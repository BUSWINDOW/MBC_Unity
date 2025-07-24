using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class ZombieSpawner : MonoBehaviourPun, IPunObservable
{
    public GameObject zombiePrefab;

    public ZombieData[] zombieDatas; // ���� ������ �迭
    public Transform[] spawnPoints; // ���� ������ ��ġ

    private List<Zombie> zombiePool = new List<Zombie>(); // ������ ���� Ǯ��
    private List<Zombie> zombies = new List<Zombie>(); // ������ ���� ���

    private int wave; // ���� ���̺�
    private int zombieCount; // ���� ���̺��� ���� ��


    private void Awake()
    {
        PhotonPeer.RegisterType(typeof(Color), 128, ColorSerialization.SerializeColor, ColorSerialization.DeserializeColor);
        // Pun2���� Color Ÿ���� RPC�޼����� �Է����� ÷�� �� �� ����.
        // ������ RPC���� �������� ���� Ÿ���� ���� �����ϵ��� ����
        // PhotonPeer.RegisterType(Ÿ��, ��ȣ, ����ȭ �޼���, ������ȭ �޼���)
        // 
        //PhotonPeer.RegisterType(typeof(ZombieData), 128, ZombieData.Serialize, ZombieData.Deserialize);
    }

    private void Start()
    {
        this.zombieDatas = Resources.LoadAll<ZombieData>("Scriptable"); // Resources �������� ���� ������ �ε�
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(wave); // ���� ���̺� ����
            stream.SendNext(zombies.Count); // ���� ���� �� ����
        }
        else
        {
            this.wave = (int)stream.ReceiveNext(); // ���� ���̺� ����
            this.zombieCount = (int)stream.ReceiveNext(); // ���� ���̺��� ���� �� ����
        }
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

            if (zombies.Count <= 0)
            {
                SpawnWave();
            }
        }
        UpdateUI(); // UI ������Ʈ

    }

    private void UpdateUI()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (GameManager.Instance != null && UIManager.Instance != null)
            {
                UIManager.Instance.UpdateWaveTxt(wave, this.zombies.Count);
            }
        }
        else // Ŭ���̾�Ʈ(������ �ƴҶ�)
        {
            if (GameManager.Instance != null && UIManager.Instance != null)
            {
                //���� ����� �����ؼ� ������ �ȵǴ� ���� ��Ʈ��ũ������ ���� ���ڷ� ����
                UIManager.Instance.UpdateWaveTxt(wave, this.zombieCount);
            }
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
                //zombiePool[i].Setup(data); // ���� ������ ���� , �̱� ���� ����
                //��Ƽ ���� ����
                zombiePool[i].photonView.RPC("Setup", RpcTarget.All, data.hp,data.damage,data.moveSpeed,data.skinColor); // RPC�� ���� ���� ������ ����
                this.zombies.Add(zombiePool[i]); // ������ ���� ��Ͽ� �߰�
                //zombiePool[i].gameObject.SetActive(true); // ��Ȱ��ȭ�� ���� �ִٸ� Ȱ��ȭ
                
                return;
            }
        }
        //����� ������ ���� Ȱ��ȭ �Ǿ��ְų�, �����Ȱ� ���ų�
        #region �̱� ���� ����, ���� ������Ʈ Ǯ��
        /*Zombie zombie = Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity).GetComponent<Zombie>(); // ���� ����
        zombie.Setup(data); // ���� ������ ����*/
        #endregion
        #region ��Ƽ ���� ����, ���� ������Ʈ Ǯ��
        Zombie zombie = PhotonNetwork.Instantiate(
            zombiePrefab.name, // ���� ������ �̸�
            spawnPoint.position, // ���� ��ġ
            Quaternion.identity // ȸ����
        ).GetComponent<Zombie>(); // Zombie ������Ʈ ��������
        zombie.photonView.RPC("Setup", RpcTarget.All, data.hp, data.damage, data.moveSpeed, data.skinColor); // RPC�� ���� ���� ������ ����

        #endregion
        zombie.DieAction += () =>
        {
            this.zombies.Remove(zombie); // ���� ������ ��Ͽ��� ����
                                         //��Ͽ����� ����
            UpdateUI(); // UI ������Ʈ
            GameManager.Instance.AddScore(100); // ���� ������Ʈ
        };

        // ������ ȣ��Ʈ(������ Ŭ���̾�Ʈ) ���� �����ϴ�, �׼� �Ҵ絵 ������ Ŭ���̾�Ʈ������ �Ǵ� ������ �ִ�
        // ���� �ذ� ���ߴ�
        this.zombiePool.Add(zombie); // ������ ���� Ǯ���� �߰�
        this.zombies.Add(zombie); // ������ ���� ��Ͽ� �߰�

    }


}
