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

    public GameObject roomItemPrefab; //방 목록
    public InputField roomName_Input; //방 이름 입력 필드
    public Transform scrollContents; // 방 목록을 넣을 부모


    private Dictionary<string, GameObject> rooms = new Dictionary<string, GameObject>(); // 방 목록을 저장할 딕셔너리

    void Awake()
    {
        if(PhotonNetwork.IsConnected) return; // 이미 접속되어 있다면 초기화 하지 않음
        PhotonNetwork.GameVersion = this.Version;
        PhotonNetwork.ConnectUsingSettings();
        //포톤 네트워크에서 접속
        
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("마스터 클라이언트 접속");
        PhotonNetwork.JoinLobby(); //로비 접속
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("로비 접속");
        //PhotonNetwork.JoinRandomRoom(); //랜덤 방 접속 시도
        userID.text = GetUserID(); //유저 아이디 가져오기
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
        print("로비 접속 실패");
        PhotonNetwork.CreateRoom("신 창 섭 니가만든 Worlds", new RoomOptions { IsOpen = true, IsVisible = true, MaxPlayers = 20}); //방 생성
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("방 접속 성공");
        //CreateTank(); //방 접속 성공 시 탱크 생성
        StartCoroutine(LoadBattleFieldScene()); // 씬을 이동하는 코루틴
    }
    IEnumerator LoadBattleFieldScene()
    {
        PhotonNetwork.IsMessageQueueRunning = false; // 씬을 이동하는 동안 클라우드 서버로부터 네트워크 메시지 수신 중단

        AsyncOperation ao = SceneManager.LoadSceneAsync(1); // 씬 비동기적으로 로딩

        yield return ao;
    }

    public void OnClickJoinRandomRoom()
    {
        PhotonNetwork.NickName = this.userID.text;
        PlayerPrefs.SetString("User_ID", this.userID.text);
        PhotonNetwork.JoinRandomRoom(); //랜덤 방 접속 시도

    }
    public void OnClickCreateRoom()
    {
        string roomName = this.roomName_Input.text;
        if (string.IsNullOrEmpty(this.roomName_Input.text)) 
        {
            roomName = $"Room {Random.Range(0,999)}";
        }
        PhotonNetwork.NickName = this.userID.text; // 아이디 입력한걸 넣음
        PlayerPrefs.SetString("User_ID", this.userID.text);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true; // 방이 열려있는 상태
        roomOptions.IsVisible = true; // 방이 보이는 상태
        roomOptions.MaxPlayers = 20; // 방 최대 플레이어 수

        //지정한 조건에 맞는 룸 생성
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default); // 방 생성
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) // 방이 새로 생기거나 삭제될시 콜백되는 함수
    {
        //삭제된 RoomItem 프리팹을 저장 할 임시 변수
        GameObject tempRoom = null;


        foreach(var info in roomList)
        {

            if (info.RemovedFromList) // 방이 삭제된 경우
            {
                this.rooms.TryGetValue(info.Name, out tempRoom); // 이걸로 뭔가 잡히면 tempRoom에 방 아이템이 들어감

                Destroy(tempRoom); // 방 아이템 삭제
                this.rooms.Remove(info.Name); // 방 아이템 딕셔너리에서 삭제
            }
            else // 방 정보가 변경된 경우
            {
                //룸 이름이 딕셔너리에 없는 경우 새로 추가
                if (!this.rooms.ContainsKey(info.Name))
                {
                    GameObject room = Instantiate(roomItemPrefab, scrollContents);
                    this.rooms[info.Name] = room; // 딕셔너리에 방 이름과 방 아이템을 추가
                    RoomData roomData = room.GetComponent<RoomData>();
                    roomData.roomName = info.Name; // 방 이름 설정
                    roomData.maxPlayers = info.MaxPlayers; // 방 최대 플레이어 수 설정
                    roomData.connectPlayers = info.PlayerCount; // 접속한 플레이어 수 설정

                    roomData.DisplayPlayerRoomData(); // 방 정보 표시

                    room.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        PhotonNetwork.NickName = this.userID.text; // 아이디 입력한걸 넣음
                        PlayerPrefs.SetString("User_ID", this.userID.text); // 아이디 저장
                        PhotonNetwork.JoinRoom(info.Name); // 방 접속
                    }); // 방 아이템 클릭 시 방 접속
                }
                else // 룸 이름이 딕셔너리에 있었던 경우
                {
                    this.rooms.TryGetValue(info.Name, out tempRoom);
                    var roomData = tempRoom.GetComponent<RoomData>();
                    //roomData.maxPlayers = info.MaxPlayers; // 방 최대 플레이어 수 설정
                    roomData.connectPlayers = info.PlayerCount; // 접속한 플레이어 수 설정
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
