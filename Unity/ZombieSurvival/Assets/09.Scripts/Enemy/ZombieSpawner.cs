using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class ZombieSpawner : MonoBehaviourPun, IPunObservable
{
    public GameObject zombiePrefab;

    public ZombieData[] zombieDatas; // 좀비 데이터 배열
    public Transform[] spawnPoints; // 좀비가 생성될 위치

    private List<Zombie> zombiePool = new List<Zombie>(); // 생성된 좀비 풀링
    private List<Zombie> zombies = new List<Zombie>(); // 생성된 좀비 목록

    private int wave; // 현재 웨이브
    private int zombieCount; // 현재 웨이브의 좀비 수


    private void Awake()
    {
        PhotonPeer.RegisterType(typeof(Color), 128, ColorSerialization.SerializeColor, ColorSerialization.DeserializeColor);
        // Pun2에서 Color 타입은 RPC메서드의 입력으로 첨부 할 수 없다.
        // 기존의 RPC에서 지원하지 않은 타입을 직접 지원하도록 만든
        // PhotonPeer.RegisterType(타입, 번호, 직렬화 메서드, 역직렬화 메서드)
        // 
        //PhotonPeer.RegisterType(typeof(ZombieData), 128, ZombieData.Serialize, ZombieData.Deserialize);
    }

    private void Start()
    {
        this.zombieDatas = Resources.LoadAll<ZombieData>("Scriptable"); // Resources 폴더에서 좀비 데이터 로드
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(wave); // 현재 웨이브 전송
            stream.SendNext(zombies.Count); // 현재 좀비 수 전송
        }
        else
        {
            this.wave = (int)stream.ReceiveNext(); // 현재 웨이브 수신
            this.zombieCount = (int)stream.ReceiveNext(); // 현재 웨이브의 좀비 수 수신
        }
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

            if (zombies.Count <= 0)
            {
                SpawnWave();
            }
        }
        UpdateUI(); // UI 업데이트

    }

    private void UpdateUI()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (GameManager.Instance != null && UIManager.Instance != null)
            {
                UIManager.Instance.UpdateWaveTxt(wave, this.zombies.Count);
            }
        }
        else // 클라이언트(마스터 아닐때)
        {
            if (GameManager.Instance != null && UIManager.Instance != null)
            {
                //직접 목록을 참고해서 갱신이 안되니 따로 네트워크상으로 받은 숫자로 갱신
                UIManager.Instance.UpdateWaveTxt(wave, this.zombieCount);
            }
        }
    }

    private void SpawnWave() // 웨이브 시작
    {
        wave++;
        int zombieCount = Mathf.RoundToInt(wave * 1.5f); // 웨이브마다 좀비 수 증가
        for (int i = 0; i < zombieCount; i++)
        {
            CreateZombie();
        }
        UpdateUI(); // UI 업데이트
    }

    private void CreateZombie()
    {
        
        ZombieData data = zombieDatas[UnityEngine.Random.Range(0, zombieDatas.Length)]; // 랜덤으로 좀비 데이터 선택
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)]; // 랜덤으로 생성 위치 선택

        for(int i = 0; i<this.zombiePool.Count; i++)
        {
            if (!zombiePool[i].gameObject.activeSelf)
            {
                zombiePool[i].transform.position = spawnPoint.position; // 생성 위치로 이동
                //zombiePool[i].Setup(data); // 좀비 데이터 설정 , 싱글 게임 기준
                //멀티 게임 기준
                zombiePool[i].photonView.RPC("Setup", RpcTarget.All, data.hp,data.damage,data.moveSpeed,data.skinColor); // RPC를 통해 좀비 데이터 설정
                this.zombies.Add(zombiePool[i]); // 생성된 좀비 목록에 추가
                //zombiePool[i].gameObject.SetActive(true); // 비활성화된 좀비가 있다면 활성화
                
                return;
            }
        }
        //여기로 왔으면 모든게 활성화 되어있거나, 생성된게 없거나
        #region 싱글 게임 기준, 좀비 오브젝트 풀링
        /*Zombie zombie = Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity).GetComponent<Zombie>(); // 좀비 생성
        zombie.Setup(data); // 좀비 데이터 설정*/
        #endregion
        #region 멀티 게임 기준, 좀비 오브젝트 풀링
        Zombie zombie = PhotonNetwork.Instantiate(
            zombiePrefab.name, // 좀비 프리팹 이름
            spawnPoint.position, // 생성 위치
            Quaternion.identity // 회전값
        ).GetComponent<Zombie>(); // Zombie 컴포넌트 가져오기
        zombie.photonView.RPC("Setup", RpcTarget.All, data.hp, data.damage, data.moveSpeed, data.skinColor); // RPC를 통해 좀비 데이터 설정

        #endregion
        zombie.DieAction += () =>
        {
            this.zombies.Remove(zombie); // 좀비가 죽으면 목록에서 제거
                                         //목록에서만 제거
            UpdateUI(); // UI 업데이트
            GameManager.Instance.AddScore(100); // 점수 업데이트
        };

        // 생성을 호스트(마스터 클라이언트) 에서 전담하니, 액션 할당도 마스터 클라이언트에서만 되는 문제가 있다
        // 아직 해결 못했다
        this.zombiePool.Add(zombie); // 생성된 좀비 풀링에 추가
        this.zombies.Add(zombie); // 생성된 좀비 목록에 추가

    }


}
