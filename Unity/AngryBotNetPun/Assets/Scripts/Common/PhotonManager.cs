using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhotonManager : MonoBehaviourPunCallbacks
{ 
    private readonly string gameVersion = "1.0";
    private string userName = "Player";

    //UI
    public TMP_InputField id_InputField;
    public TMP_InputField room_InputField;

    //�� ��Ͽ� ���� �����͸� ������ Dictionary
    private Dictionary<string, GameObject> roomsDic = new Dictionary<string, GameObject>();
    //�� ����� ǥ���� ������
    private GameObject roomItemPrefab;
    //�� �������� �¾ �θ� ��ġ(content)
    public Transform contents;

    private void Awake()
    {
        this.roomItemPrefab = Resources.Load<GameObject>("RoomItem");


        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("�̹� ����");
            return;
        }
        else
        {
            Debug.Log("ù ����");
        }
        //������ Ŭ���̾�Ʈ�� �� �ڵ� ����ȭ �ɼ�
        PhotonNetwork.AutomaticallySyncScene = true; // ����ȭ�� �� ���
        //������ ���ο� ���� ������� ��, �ش� ���� �ٸ� �����鿡�Ե� �ڵ����� �ش� ���� �ε�

        PhotonNetwork.GameVersion = gameVersion; // ���� ���� ����
        PhotonNetwork.NickName = userName; // �÷��̾� �̸� ����        

        //���� �������� �������� �ʴ� ���� Ƚ��
        print(PhotonNetwork.SendRate);

        /*if (!PhotonNetwork.IsConnected) // ���� ȭ�鿡�� �ٽ� �κ�� ���ƿ��� ��쿣 �̹� ���ӵǾ�����
                                        // �ش� ��쿣 �Ʒ��� �������� ����
        {
            PhotonNetwork.ConnectUsingSettings(); // ���� ������ ����
        }*/
        PhotonNetwork.ConnectUsingSettings(); // ���� ������ ����


    }

    private void Start()
    {
        this.userName = PlayerPrefs.GetString("User_Id", $"User_{1557}");
        id_InputField.text = userName;

        if (PhotonNetwork.IsConnected) return;

        PhotonNetwork.NickName = userName;
    }
    public void OnClickLoginBtn() // �α��� ��ư ������, ���� �̸� ��ǲ�� �� �̸� ��ǲ üũ�ؼ� �α��� ��Ű��
    {
        SetUserId();
        //PhotonNetwork.IsMessageQueueRunning = false; // LoadLevel�� ���� �̵��ϰ� �־ �̰� �������� �ʾƵ� �ȴ�.
        if (!string.IsNullOrEmpty(this.room_InputField.text)) // �� �̸����� ������
        {
            /*if (this.roomsDic.ContainsKey(this.room_InputField.text)) // �� ���� �ִٸ�
            {
                PhotonNetwork.JoinRoom(this.room_InputField.text);
            }
            else // ���ٸ�
            {
                MakeRoom(this.room_InputField.text); // �� �̸��� �� ����
            }*/
            PhotonNetwork.JoinOrCreateRoom(this.room_InputField.text, new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 20 }, TypedLobby.Default); 
        }
        else
        {
            //��������
            if (PhotonNetwork.CountOfRooms <= 0) // ���� ���ٸ�
            {
                room_InputField.text = "God Chang Seop";
                MakeRoom(this.room_InputField.text);
                //����Ʈ �� ����
            }
            else//���� �ִٸ�
            {
                PhotonNetwork.JoinRandomRoom();
            }

        }


    }

    private void SetUserId()
    {
        if (string.IsNullOrEmpty(this.id_InputField.text))
        {
            userName = $"User_{photonView.ViewID}";
        }
        else
        {
            this.userName = this.id_InputField.text;

        }
        //������ ����
        PlayerPrefs.SetString("User_Id", this.userName);
        //���� ������ �г��� ���
        PhotonNetwork.NickName = userName;
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
        //PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //���� ������ ���� ����
        Debug.Log($"Failed to join random room: {message} , {returnCode}");
        MakeRoom("God Chang Seop");
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

        //������ Ŭ���̾�Ʈ�� ���
        if (PhotonNetwork.IsMasterClient)
        {
            //������ Ŭ���̾�Ʈ�� ������ ���� �ش� ���� �ٸ� �����鵵 �� ���� �ü��ֵ���
            PhotonNetwork.LoadLevel(1); // AngryBotScene �� ��ȣ  : 1
        }

    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        GameObject tempRoom = null; // ������ ���� ������ ��� ���� ��


        //���⿡ ���Դٴ°� ���Ͱ� ���� �� ������ �ٲ����
        foreach (var roomInfo in roomList) //��� ��ü üũ
        {
            if (roomInfo.RemovedFromList)//�� ���� ������ ���� �ִٸ�(�ȿ� ������ 0���� �ǰų� �ؼ�)
            {
                roomsDic.TryGetValue(roomInfo.Name, out tempRoom); // Dictionary�� �̿��ؼ�, key���� string�� �̿��ؼ� �� ���� �ִ��� ã�´�
                Destroy(tempRoom); // �� ���� GameObject����(ȭ�鿡�� �Ⱥ��̰� �ȴ�)
                this.roomsDic.Remove(roomInfo.Name); // dictionary������ ����
            }
            else//���� ������ �ƴµ� �װ� ������ �ƴ϶��
            {
                if (!roomsDic.ContainsKey(roomInfo.Name)) // dictionary�� �� ���� �ִ��� üũ�ؼ�
                                                          // ���ٸ� -> �ű� �߰��� ��
                {
                    GameObject room = Instantiate(this.roomItemPrefab,this.contents);
                    var data = room.GetComponent<RoomData>();

                    data.RoomInfo = roomInfo; // ������Ƽ ������ �Ǿ��ֱ� ������ �� ��ư�� Text�� �� �ڵ忡�� �˾Ƽ� �ٲ�

                    data.roomBtn.onClick.AddListener(() =>
                    {
                        SetUserId();
                        PhotonNetwork.JoinRoom(roomInfo.Name);
                    });
                    this.roomsDic.Add(roomInfo.Name, room);
                }
                else //������ �ƴµ� �̹� �ִ� ���� -> �ο��� ���� �ٲ�
                {
                    roomsDic.TryGetValue(roomInfo.Name, out tempRoom);
                    tempRoom.GetComponent<RoomData>().RoomInfo = roomInfo;
                }
            }

        }
    }


    private void MakeRoom(string roomName)
    {
        RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 20 };
        
        PhotonNetwork.CreateRoom(roomName, options, TypedLobby.Default); // �� ����
    }
}
