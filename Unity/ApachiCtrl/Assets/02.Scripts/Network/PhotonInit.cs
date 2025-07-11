using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.UI;
public class PhotonInit : MonoBehaviourPunCallbacks
{
    public string Version = "1.0";

    public InputField id_Input;
    public Text userID;

    void Awake()
    {
        PhotonNetwork.GameVersion = this.Version;
        PhotonNetwork.ConnectUsingSettings();
        //���� ��Ʈ��ũ���� ����
        
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("������ Ŭ���̾�Ʈ ����");
        PhotonNetwork.JoinLobby(); //�κ� ����
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("�κ� ����");
        //PhotonNetwork.JoinRandomRoom(); //���� �� ���� �õ�
        userID.text = GetUserID(); //���� ���̵� ��������
    }
    string GetUserID()
    {
        string userID = PlayerPrefs.GetString("User_ID");

        if (string.IsNullOrEmpty(userID))
        {
            userID = $"User_{Random.Range(0, 999)}";
        }
        
        return userID;
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        print("�κ� ���� ����");
        PhotonNetwork.CreateRoom("�� â �� �ϰ����� Worlds", new RoomOptions { IsOpen = true, IsVisible = true, MaxPlayers = 20}); //�� ����
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("�� ���� ����");
        //CreateTank(); //�� ���� ���� �� ��ũ ����
        StartCoroutine(LoadBattleFieldScene()); // ���� �̵��ϴ� �ڷ�ƾ
    }
    IEnumerator LoadBattleFieldScene()
    {
        PhotonNetwork.IsMessageQueueRunning = false; // ���� �̵��ϴ� ���� Ŭ���� �����κ��� ��Ʈ��ũ �޽��� ���� �ߴ�

        AsyncOperation ao = SceneManager.LoadSceneAsync(1); // �� �񵿱������� �ε�

        yield return ao;
    }

    public void OnClickJoinRandomRoom()
    {
        PhotonNetwork.NickName = this.userID.text;
        PlayerPrefs.SetString("User_ID", this.userID.text);
        PhotonNetwork.JoinRandomRoom(); //���� �� ���� �õ�

    }


    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }

}
