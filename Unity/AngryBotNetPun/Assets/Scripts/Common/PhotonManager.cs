using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{ 
    private readonly string gameVersion = "1.0";
    private string userName = "Player";
    private void Awake()
    {
        //������ Ŭ���̾�Ʈ�� �� �ڵ� ����ȭ �ɼ�
        PhotonNetwork.AutomaticallySyncScene = true; // ����ȭ�� �� ���
        //������ ���ο� ���� ������� ��,



        PhotonNetwork.GameVersion = gameVersion; // ���� ���� ����
        PhotonNetwork.NickName = userName; // �÷��̾� �̸� ����        

        //���� �������� �������� �ʴ� ���� Ƚ��
        print(PhotonNetwork.SendRate);

        PhotonNetwork.ConnectUsingSettings(); // ���� ������ ����
    }

    public override void OnConnectedToMaster()
    {


        Debug.Log($"Connected to Master\n" +
            $"{PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby(); // �κ� ����
    }
    public override void OnJoinedLobby()
    {
        Debug.Log($"Connected to Master\n" +
    $"{PhotonNetwork.InLobby}");
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //���� ������ ���� ����
        Debug.Log($"Failed to join random room: {message} , {returnCode}");
        RoomOptions options = new RoomOptions() { IsOpen = true , IsVisible = true, MaxPlayers = 20};
        PhotonNetwork.CreateRoom("God Chang Sub", options , TypedLobby.Default); // �� ����
    }
    public override void OnCreatedRoom()
    {
        //�� ���� �Ϸ� �� ȣ��Ǵ� �ݹ� �Լ�
        Debug.Log("�� ����");
        Debug.Log($"�� �̸�: {PhotonNetwork.CurrentRoom.Name}");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("�濡 �����߽��ϴ�.");
        Debug.Log($"{PhotonNetwork.InRoom}");
        Debug.Log($"{PhotonNetwork.CurrentRoom.PlayerCount}");

        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"�÷��̾� �̸�: {player.Value.NickName}, �÷��̾� ID:{player.Value.ActorNumber} ");
            //NickName�� ������ ���� �ƴ϶� ������ �г����� ������ �� �ִ�.
        }

        

    }

    private static void CreatePlayer()
    {
        Transform[] points = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        PhotonNetwork.Instantiate
            ("Player",
            points[idx].position,
            Quaternion.identity
            );
    }
}
