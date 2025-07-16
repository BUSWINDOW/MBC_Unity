using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>(); // 해당 씬에 있는 UIManager를 찾음
            }
            return _instance;
        }
    } // 싱글톤 인스턴스
    private static GameManager _instance; // 싱글톤 인스턴스 변수
    public bool IsGameOver { get; private set; }
    private int score = 0; // 게임 점수


    private void Awake()
    {
        if(Instance != this)
        {
            Destroy(this.gameObject); // 싱글톤 인스턴스가 이미 존재하면 현재 오브젝트를 파괴
        }
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        PhotonNetwork.Instantiate(
            "Woman", 
            new Vector3(0, 3f, 0), 
            Quaternion.identity, 
            0, 
            null);
    }

    void Start()
    {
        FindObjectOfType<WomanHealth>().DieAction += this.OnGameOver; // 플레이어가 죽었을 때 게임 오버 처리
    }

    private void OnGameOver()
    {
        this.IsGameOver = true; // 게임 오버 상태로 변경
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowGameOverUI(true); // 게임 오버 패널 표시
        }
    }
    public void AddScore(int score)
    {
        if(this.IsGameOver) return; // 게임 오버 상태면 점수 추가하지 않음
        this.score += score; // 점수 추가
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateScoreTxt(this.score); // UI 업데이트
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PhotonNetwork.LeaveRoom(); // 방을 나갈 때 ESC 키를 누르면 방을 나감
        }
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0); // 방을 나가면 로비 씬으로 이동
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this.score); // 점수를 전송
        }
        else
        {
            this.score = (int)stream.ReceiveNext(); // 점수를 수신
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateScoreTxt(this.score); // UI 업데이트
            }
        }
    }
}
