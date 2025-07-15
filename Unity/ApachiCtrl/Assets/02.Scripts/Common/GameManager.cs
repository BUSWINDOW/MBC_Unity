using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance { get; private set; }
    public bool isGameOver = false;

    public Text playerCntText; // 플레이어 수 표시용 텍스트
    public Text logMsgText; // 로그 메시지 표시용 텍스트

    [SerializeField] private GameObject apachePrefab;
    [SerializeField]private List<Transform> spawnList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        PhotonNetwork.IsMessageQueueRunning = true; //다시 네트워크로부터 메세지 받게 설정
        CreateTank();
        GameObject.Find("SpawnPoints").transform.GetComponentsInChildren<Transform>(spawnList); // 스폰 포인트 리스트 초기화
        spawnList.RemoveAt(0); // 첫 번째 요소는 제거 (Transform 컴포넌트가 있는 GameObject 자체)

    }
    void CreateTank()
    {
        float pos = Random.Range(-80f, 80f);
        PhotonNetwork.Instantiate(
            "Tank",
            new Vector3(pos, 3f, pos),
            Quaternion.identity,
            0,
            null);
    }
    void CreateApache()
    {
        if (isGameOver) return;
        int count = (int)GameObject.FindGameObjectsWithTag("Apache").Length; // 현재 생성된 탱크의 수를 확인
        if(count < 10)
        {
            int idx = Random.Range(0, spawnList.Count); // 랜덤으로 스폰 포인트 선택
            Vector3 spawnPos = spawnList[idx].position; // 선택된 스폰 포인트의 위치
            PhotonNetwork.InstantiateRoomObject(
                apachePrefab.name,
                spawnPos,
                Quaternion.identity,
                0,
                null); // 아파치 생성
        }
    }
    private void Start()
    {
        string msg = "\n<color=#00ff00>[" + PhotonNetwork.NickName + "]</color> 님이 접속하셨습니다.";

        photonView.RPC("LogMsg", RpcTarget.AllBuffered, msg); // 모든 클라이언트에게 로그 메시지 전송

        if (spawnList.Count > 0 && PhotonNetwork.IsMasterClient)
            InvokeRepeating("CreateApache", 0.2f, 3f); // 5초마다 아파치 생성
    }
    [PunRPC]
    void LogMsg(string log)
    {
        this.logMsgText.text += log; // 로그 메시지 추가
    }
    [PunRPC]
    public void ApplyPlayerCountUpdate() // 포톤 네트워크상의 현재 룸을 반환받음
    {
        Room currentRoom = PhotonNetwork.CurrentRoom;
        this.playerCntText.text = $"{currentRoom.PlayerCount}/{currentRoom.MaxPlayers}"; // 플레이어 수 표시
    }
    [PunRPC]
    void GetConnectCnt()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ApplyPlayerCountUpdate", RpcTarget.All); // 모든 클라이언트에게 플레이어 수 업데이트 요청
            //버그남
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer) // 유저가 입장했을때 자동 호출
    {
        this.GetConnectCnt();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer) // 유저가 퇴장했을때 자동 호출
    {
        this.GetConnectCnt();
    }
    public void ExitBattle()
    {
        string msg = "\n<color=#ff0000>[" + PhotonNetwork.NickName + "]</color> 님이 퇴장하셨습니다.";

        photonView.RPC("LogMsg", RpcTarget.AllBuffered, msg); // 모든 클라이언트에게 로그 메시지 전송

        PhotonNetwork.LeaveRoom(); // 현재 방을 나감
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0); // 메인 메뉴 씬으로 이동
    }
}
