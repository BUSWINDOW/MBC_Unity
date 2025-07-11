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


    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }

}
