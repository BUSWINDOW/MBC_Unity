using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1.0";
    public Text connectInfoTxt;
    public Button joinBtn;
    private void Start()
    {
        PhotonNetwork.GameVersion = gameVersion; //���� ���� ����
        PhotonNetwork.ConnectUsingSettings(); //Photon ���� ����
        this.joinBtn.interactable = false; //���� ��ư ��Ȱ��ȭ
        this.connectInfoTxt.text = "���� ���� ��..."; //���� ���� �ؽ�Ʈ �ʱ�ȭ
    }
    public void Connect()
    {
        //���� ��ư�� �����ؼ� ���ӹ�ư Ŭ���� ����
        this.joinBtn.interactable = false; //���� ��ư ��Ȱ��ȭ
        if (PhotonNetwork.IsConnected)
        {
            this.connectInfoTxt.text = "�濡 ���� ��..."; //���� ���� �ؽ�Ʈ ������Ʈ
        }
        else
        {
            this.connectInfoTxt.text = "���� ����, ���� ��õ�";
            PhotonNetwork.ConnectUsingSettings(); //Photon ���� ���� ��õ�
        }

    }
    public override void OnConnectedToMaster()
    {
        //������ ���� ���� ������ �ڵ� ����
        this.joinBtn.interactable = true; //���� ��ư Ȱ��ȭ
        this.connectInfoTxt.text = "���� ���� ����"; //���� ���� �ؽ�Ʈ ������Ʈ
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        //���� ���� ���н� �ڵ� ����
        this.joinBtn.interactable = false; //���� ��ư ��Ȱ��ȭ
        connectInfoTxt.text = "���� ���� ����";
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //�� ���� ���� �� ���� ����
        this.connectInfoTxt.text = "���� ���� ���ο� �� ����"; //���� ���� �ؽ�Ʈ ������Ʈ
        PhotonNetwork.CreateRoom(null, new RoomOptions { IsOpen = true, IsVisible = true ,  MaxPlayers = 4 }, TypedLobby.Default); //���ο� �� ����
    }
    public override void OnJoinedRoom()
    {
        this.connectInfoTxt.text = "�� ���� ����";
        //�뿡 ���� ������ �ڵ� ����

        // �뿡 ������ ��� �����ڰ� MainScene���� �̵��ϰ� ����
        PhotonNetwork.LoadLevel("MainScene");

    }

}
