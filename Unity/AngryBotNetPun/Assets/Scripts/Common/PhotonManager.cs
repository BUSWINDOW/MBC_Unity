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

    //룸 목록에 대한 데이터를 저장할 Dictionary
    private Dictionary<string, GameObject> roomsDic = new Dictionary<string, GameObject>();
    //룸 목록을 표시할 프리팹
    private GameObject roomItemPrefab;
    //그 프리팹이 태어날 부모 위치(content)
    public Transform contents;

    private void Awake()
    {
        this.roomItemPrefab = Resources.Load<GameObject>("RoomItem");


        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("이미 연결");
            return;
        }
        else
        {
            Debug.Log("첫 연결");
        }
        //마스터 클라이언트의 씬 자동 동기화 옵션
        PhotonNetwork.AutomaticallySyncScene = true; // 동기화된 씬 사용
        //방장이 새로운 씬을 만들었을 때, 해당 룸의 다른 유저들에게도 자동으로 해당 씬을 로딩

        PhotonNetwork.GameVersion = gameVersion; // 게임 버전 설정
        PhotonNetwork.NickName = userName; // 플레이어 이름 설정        

        //포톤 서버와의 데이터의 초당 전송 횟수
        print(PhotonNetwork.SendRate);

        /*if (!PhotonNetwork.IsConnected) // 게임 화면에서 다시 로비로 돌아오는 경우엔 이미 접속되어있음
                                        // 해당 경우엔 아래를 실행하지 않음
        {
            PhotonNetwork.ConnectUsingSettings(); // 포톤 서버에 연결
        }*/
        PhotonNetwork.ConnectUsingSettings(); // 포톤 서버에 연결


    }

    private void Start()
    {
        this.userName = PlayerPrefs.GetString("User_Id", $"User_{1557}");
        id_InputField.text = userName;

        if (PhotonNetwork.IsConnected) return;

        PhotonNetwork.NickName = userName;
    }
    public void OnClickLoginBtn() // 로그인 버튼 눌러서, 유저 이름 인풋과 방 이름 인풋 체크해서 로그인 시키기
    {
        SetUserId();
        //PhotonNetwork.IsMessageQueueRunning = false; // LoadLevel로 씬을 이동하고 있어서 이걸 포함하지 않아도 된다.
        if (!string.IsNullOrEmpty(this.room_InputField.text)) // 방 이름까지 쳤을때
        {
            /*if (this.roomsDic.ContainsKey(this.room_InputField.text)) // 그 방이 있다면
            {
                PhotonNetwork.JoinRoom(this.room_InputField.text);
            }
            else // 없다면
            {
                MakeRoom(this.room_InputField.text); // 그 이름의 방 생성
            }*/
            PhotonNetwork.JoinOrCreateRoom(this.room_InputField.text, new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 20 }, TypedLobby.Default); 
        }
        else
        {
            //안쳤을때
            if (PhotonNetwork.CountOfRooms <= 0) // 방이 없다면
            {
                room_InputField.text = "God Chang Seop";
                MakeRoom(this.room_InputField.text);
                //디폴트 방 생성
            }
            else//방이 있다면
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
        //유저명 저장
        PlayerPrefs.SetString("User_Id", this.userName);
        //접속 유저의 닉네임 등록
        PhotonNetwork.NickName = userName;
    }

    public override void OnConnectedToMaster()
    {


        Debug.Log($"Connected to Master\n" +
            $"{PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby(); // 로비에 참여
    }
    public override void OnJoinedLobby()
    {
        Debug.Log($"Connected to Master\n" +
    $"{PhotonNetwork.InLobby}");
        //PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //입장 실패한 이유 설명
        Debug.Log($"Failed to join random room: {message} , {returnCode}");
        MakeRoom("God Chang Seop");
    }



    public override void OnCreatedRoom()
    {
        //룸 생성 완료 후 호출되는 콜백 함수
        Debug.Log("방 생성");
        Debug.Log($"방 이름: {PhotonNetwork.CurrentRoom.Name}");

    }
    public override void OnJoinedRoom()
    {
        Debug.Log("방에 입장했습니다.");
        Debug.Log($"{PhotonNetwork.InRoom}");
        Debug.Log($"{PhotonNetwork.CurrentRoom.PlayerCount}");

        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"플레이어 이름: {player.Value.NickName}, 플레이어 ID:{player.Value.ActorNumber} ");
            //NickName은 고유의 값이 아니라서 동일한 닉네임이 존재할 수 있다.
        }

        //마스터 클라이언트의 경우
        if (PhotonNetwork.IsMasterClient)
        {
            //마스터 클라이언트가 접속한 씬을 해당 방의 다른 유저들도 다 같이 올수있도록
            PhotonNetwork.LoadLevel(1); // AngryBotScene 의 번호  : 1
        }

    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        GameObject tempRoom = null; // 삭제된 방의 정보를 담기 위한 것


        //여기에 들어왔다는거 부터가 뭔가 룸 정보는 바뀐거임
        foreach (var roomInfo in roomList) //룸들 전체 체크
        {
            if (roomInfo.RemovedFromList)//이 방이 삭제된 적이 있다면(안에 유저가 0명이 되거나 해서)
            {
                roomsDic.TryGetValue(roomInfo.Name, out tempRoom); // Dictionary를 이용해서, key값인 string을 이용해서 그 방이 있는지 찾는다
                Destroy(tempRoom); // 그 방의 GameObject삭제(화면에서 안보이게 된다)
                this.roomsDic.Remove(roomInfo.Name); // dictionary에서도 제거
            }
            else//뭔가 변경은 됐는데 그게 삭제는 아니라면
            {
                if (!roomsDic.ContainsKey(roomInfo.Name)) // dictionary에 그 방이 있는지 체크해서
                                                          // 없다면 -> 신규 추가된 방
                {
                    GameObject room = Instantiate(this.roomItemPrefab,this.contents);
                    var data = room.GetComponent<RoomData>();

                    data.RoomInfo = roomInfo; // 프로퍼티 식으로 되어있기 때문에 방 버튼의 Text는 이 코드에서 알아서 바뀜

                    data.roomBtn.onClick.AddListener(() =>
                    {
                        SetUserId();
                        PhotonNetwork.JoinRoom(roomInfo.Name);
                    });
                    this.roomsDic.Add(roomInfo.Name, room);
                }
                else //변경은 됐는데 이미 있던 방임 -> 인원수 등이 바뀜
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
        
        PhotonNetwork.CreateRoom(roomName, options, TypedLobby.Default); // 방 생성
    }
}
