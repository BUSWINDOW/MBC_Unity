using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance { get; private set; }
    public bool isGameOver = false;

    public Text playerCntText; // �÷��̾� �� ǥ�ÿ� �ؽ�Ʈ
    public Text logMsgText; // �α� �޽��� ǥ�ÿ� �ؽ�Ʈ

    [SerializeField] private GameObject apachePrefab;
    [SerializeField]private List<Transform> spawnList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        PhotonNetwork.IsMessageQueueRunning = true; //�ٽ� ��Ʈ��ũ�κ��� �޼��� �ް� ����
        CreateTank();
        GameObject.Find("SpawnPoints").transform.GetComponentsInChildren<Transform>(spawnList); // ���� ����Ʈ ����Ʈ �ʱ�ȭ
        spawnList.RemoveAt(0); // ù ��° ��Ҵ� ���� (Transform ������Ʈ�� �ִ� GameObject ��ü)

    }
    void CreateTank()
    {
        float pos = Random.Range(-80f, 80f);
        PhotonNetwork.Instantiate(
            "Tank",
            new Vector3(pos, 3f, pos),
            Quaternion.identity,
            0,
            null);
    }
    void CreateApache()
    {
        if (isGameOver) return;
        int count = (int)GameObject.FindGameObjectsWithTag("Apache").Length; // ���� ������ ��ũ�� ���� Ȯ��
        if(count < 10)
        {
            int idx = Random.Range(0, spawnList.Count); // �������� ���� ����Ʈ ����
            Vector3 spawnPos = spawnList[idx].position; // ���õ� ���� ����Ʈ�� ��ġ
            PhotonNetwork.InstantiateRoomObject(
                apachePrefab.name,
                spawnPos,
                Quaternion.identity,
                0,
                null); // ����ġ ����
        }
    }
    private void Start()
    {
        string msg = "\n<color=#00ff00>[" + PhotonNetwork.NickName + "]</color> ���� �����ϼ̽��ϴ�.";

        photonView.RPC("LogMsg", RpcTarget.AllBuffered, msg); // ��� Ŭ���̾�Ʈ���� �α� �޽��� ����

        if (spawnList.Count > 0 && PhotonNetwork.IsMasterClient)
            InvokeRepeating("CreateApache", 0.2f, 3f); // 5�ʸ��� ����ġ ����
    }
    [PunRPC]
    void LogMsg(string log)
    {
        this.logMsgText.text += log; // �α� �޽��� �߰�
    }
    [PunRPC]
    public void ApplyPlayerCountUpdate() // ���� ��Ʈ��ũ���� ���� ���� ��ȯ����
    {
        Room currentRoom = PhotonNetwork.CurrentRoom;
        this.playerCntText.text = $"{currentRoom.PlayerCount}/{currentRoom.MaxPlayers}"; // �÷��̾� �� ǥ��
    }
    [PunRPC]
    void GetConnectCnt()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ApplyPlayerCountUpdate", RpcTarget.All); // ��� Ŭ���̾�Ʈ���� �÷��̾� �� ������Ʈ ��û
            //���׳�
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer) // ������ ���������� �ڵ� ȣ��
    {
        this.GetConnectCnt();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer) // ������ ���������� �ڵ� ȣ��
    {
        this.GetConnectCnt();
    }
    public void ExitBattle()
    {
        string msg = "\n<color=#ff0000>[" + PhotonNetwork.NickName + "]</color> ���� �����ϼ̽��ϴ�.";

        photonView.RPC("LogMsg", RpcTarget.AllBuffered, msg); // ��� Ŭ���̾�Ʈ���� �α� �޽��� ����

        PhotonNetwork.LeaveRoom(); // ���� ���� ����
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0); // ���� �޴� ������ �̵�
    }
}
