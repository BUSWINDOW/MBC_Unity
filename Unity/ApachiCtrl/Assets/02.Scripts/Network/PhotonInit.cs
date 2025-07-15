using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.UI;
using ExitGames.Client.Photon;
public class PhotonInit : MonoBehaviourPunCallbacks
{
    public string Version = "1.0";

    public InputField id_Input;
    public Text userID;

    public GameObject roomItemPrefab; //�� ���
    public InputField roomName_Input; //�� �̸� �Է� �ʵ�
    public Transform scrollContents; // �� ����� ���� �θ�


    private Dictionary<string, GameObject> rooms = new Dictionary<string, GameObject>(); // �� ����� ������ ��ųʸ�

    void Awake()
    {
        if(PhotonNetwork.IsConnected) return; // �̹� ���ӵǾ� �ִٸ� �ʱ�ȭ ���� ����
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
    public void OnClickCreateRoom()
    {
        string roomName = this.roomName_Input.text;
        if (string.IsNullOrEmpty(this.roomName_Input.text)) 
        {
            roomName = $"Room {Random.Range(0,999)}";
        }
        PhotonNetwork.NickName = this.userID.text; // ���̵� �Է��Ѱ� ����
        PlayerPrefs.SetString("User_ID", this.userID.text);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true; // ���� �����ִ� ����
        roomOptions.IsVisible = true; // ���� ���̴� ����
        roomOptions.MaxPlayers = 20; // �� �ִ� �÷��̾� ��

        //������ ���ǿ� �´� �� ����
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default); // �� ����
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) // ���� ���� ����ų� �����ɽ� �ݹ�Ǵ� �Լ�
    {
        //������ RoomItem �������� ���� �� �ӽ� ����
        GameObject tempRoom = null;


        foreach(var info in roomList)
        {

            if (info.RemovedFromList) // ���� ������ ���
            {
                this.rooms.TryGetValue(info.Name, out tempRoom); // �̰ɷ� ���� ������ tempRoom�� �� �������� ��

                Destroy(tempRoom); // �� ������ ����
                this.rooms.Remove(info.Name); // �� ������ ��ųʸ����� ����
            }
            else // �� ������ ����� ���
            {
                //�� �̸��� ��ųʸ��� ���� ��� ���� �߰�
                if (!this.rooms.ContainsKey(info.Name))
                {
                    GameObject room = Instantiate(roomItemPrefab, scrollContents);
                    this.rooms[info.Name] = room; // ��ųʸ��� �� �̸��� �� �������� �߰�
                    RoomData roomData = room.GetComponent<RoomData>();
                    roomData.roomName = info.Name; // �� �̸� ����
                    roomData.maxPlayers = info.MaxPlayers; // �� �ִ� �÷��̾� �� ����
                    roomData.connectPlayers = info.PlayerCount; // ������ �÷��̾� �� ����

                    roomData.DisplayPlayerRoomData(); // �� ���� ǥ��

                    room.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        PhotonNetwork.NickName = this.userID.text; // ���̵� �Է��Ѱ� ����
                        PlayerPrefs.SetString("User_ID", this.userID.text); // ���̵� ����
                        PhotonNetwork.JoinRoom(info.Name); // �� ����
                    }); // �� ������ Ŭ�� �� �� ����
                }
                else // �� �̸��� ��ųʸ��� �־��� ���
                {
                    this.rooms.TryGetValue(info.Name, out tempRoom);
                    var roomData = tempRoom.GetComponent<RoomData>();
                    //roomData.maxPlayers = info.MaxPlayers; // �� �ִ� �÷��̾� �� ����
                    roomData.connectPlayers = info.PlayerCount; // ������ �÷��̾� �� ����
                    roomData.DisplayPlayerRoomData();

                }
            }

            


        }
        
    }


    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }

}
