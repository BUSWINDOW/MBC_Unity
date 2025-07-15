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
        PhotonNetwork.GameVersion = gameVersion; //게임 버전 설정
        PhotonNetwork.ConnectUsingSettings(); //Photon 서버 접속
        this.joinBtn.interactable = false; //접속 버튼 비활성화
        this.connectInfoTxt.text = "서버 접속 중..."; //접속 정보 텍스트 초기화
    }
    public void Connect()
    {
        //접속 버튼에 연결해서 접속버튼 클릭시 실행
        this.joinBtn.interactable = false; //접속 버튼 비활성화
        if (PhotonNetwork.IsConnected)
        {
            this.connectInfoTxt.text = "방에 접속 중..."; //접속 정보 텍스트 업데이트
        }
        else
        {
            this.connectInfoTxt.text = "접속 실패, 접속 재시도";
            PhotonNetwork.ConnectUsingSettings(); //Photon 서버 접속 재시도
        }

    }
    public override void OnConnectedToMaster()
    {
        //마스터 서버 접속 성공시 자동 실행
        this.joinBtn.interactable = true; //접속 버튼 활성화
        this.connectInfoTxt.text = "서버 접속 성공"; //접속 정보 텍스트 업데이트
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        //서버 접속 실패시 자동 실행
        this.joinBtn.interactable = false; //접속 버튼 비활성화
        connectInfoTxt.text = "서버 접속 실패";
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //빈 방이 없어 방 참가 실패
        this.connectInfoTxt.text = "방이 없어 새로운 방 생성"; //접속 정보 텍스트 업데이트
        PhotonNetwork.CreateRoom(null, new RoomOptions { IsOpen = true, IsVisible = true ,  MaxPlayers = 4 }, TypedLobby.Default); //새로운 방 생성
    }
    public override void OnJoinedRoom()
    {
        this.connectInfoTxt.text = "룸 참가 성공";
        //룸에 참가 성공시 자동 실행

        // 룸에 참가한 모든 참가자가 MainScene으로 이동하게 설정
        PhotonNetwork.LoadLevel("MainScene");

    }

}
