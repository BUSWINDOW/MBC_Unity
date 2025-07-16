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
                _instance = FindObjectOfType<GameManager>(); // �ش� ���� �ִ� UIManager�� ã��
            }
            return _instance;
        }
    } // �̱��� �ν��Ͻ�
    private static GameManager _instance; // �̱��� �ν��Ͻ� ����
    public bool IsGameOver { get; private set; }
    private int score = 0; // ���� ����


    private void Awake()
    {
        if(Instance != this)
        {
            Destroy(this.gameObject); // �̱��� �ν��Ͻ��� �̹� �����ϸ� ���� ������Ʈ�� �ı�
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
        FindObjectOfType<WomanHealth>().DieAction += this.OnGameOver; // �÷��̾ �׾��� �� ���� ���� ó��
    }

    private void OnGameOver()
    {
        this.IsGameOver = true; // ���� ���� ���·� ����
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowGameOverUI(true); // ���� ���� �г� ǥ��
        }
    }
    public void AddScore(int score)
    {
        if(this.IsGameOver) return; // ���� ���� ���¸� ���� �߰����� ����
        this.score += score; // ���� �߰�
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateScoreTxt(this.score); // UI ������Ʈ
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PhotonNetwork.LeaveRoom(); // ���� ���� �� ESC Ű�� ������ ���� ����
        }
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0); // ���� ������ �κ� ������ �̵�
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this.score); // ������ ����
        }
        else
        {
            this.score = (int)stream.ReceiveNext(); // ������ ����
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateScoreTxt(this.score); // UI ������Ʈ
            }
        }
    }
}
