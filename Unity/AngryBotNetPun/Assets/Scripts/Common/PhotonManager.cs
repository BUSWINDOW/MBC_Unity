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
        //마스터 클라이언트의 씬 자동 동기화 옵션
        PhotonNetwork.AutomaticallySyncScene = true; // 동기화된 씬 사용
        //방장이 새로운 씬을 만들었을 때,



        PhotonNetwork.GameVersion = gameVersion; // 게임 버전 설정
        PhotonNetwork.NickName = userName; // 플레이어 이름 설정        

        //포톤 서버와의 데이터의 초당 전송 횟수
        print(PhotonNetwork.SendRate);

        PhotonNetwork.ConnectUsingSettings(); // 포톤 서버에 연결
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
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //입장 실패한 이유 설명
        Debug.Log($"Failed to join random room: {message} , {returnCode}");
        RoomOptions options = new RoomOptions() { IsOpen = true , IsVisible = true, MaxPlayers = 20};
        PhotonNetwork.CreateRoom("God Chang Sub", options , TypedLobby.Default); // 방 생성
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
