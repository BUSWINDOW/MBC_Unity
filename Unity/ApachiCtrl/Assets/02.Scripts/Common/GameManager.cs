using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool isGameOver = false;
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
}
