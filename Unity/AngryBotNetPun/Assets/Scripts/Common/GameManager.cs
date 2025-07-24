using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;
    public Volume volume;
    DepthOfField dof;

    public TMP_Text roomNameTxt;
    public TMP_Text playerCntTxt;
    public TMP_Text log_Txt;
    public Button exitBtn;
    IEnumerator Start()
    {
        Instance = this;
        this.volume.profile.TryGet<DepthOfField>(out dof);
        while (!PhotonNetwork.InRoom)
        {
            yield return null;
        }
        //PhotonNetwork.IsMessageQueueRunning = true; // LoadLevel�� �̵��ϰ� �־ �� �ڵ带 �������� �ʾƵ� �ȴ�. 
        CreatePlayer();
        this.exitBtn.onClick.AddListener(() =>
        {
            dof.active = true;
            //PhotonNetwork.LeaveRoom();
            //StartCoroutine(LeaveRoomSmooth());
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(0);
        });
    }


    private void CreatePlayer()
    {

        Transform[] points = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        var player = PhotonNetwork.Instantiate
            ("Player",
            points[idx].position,
            Quaternion.identity
            );
        player.GetComponent<PlayerHealth>().dieAction += (bullet) =>
        {
            var num = bullet.GetComponent<BulletCtrl>().actorNum;
            var killerName = PhotonNetwork.CurrentRoom.Players[num].NickName;
            var victimName = PhotonNetwork.CurrentRoom.Players[player.GetPhotonView().OwnerActorNr].NickName;
            this.log_Txt.text += $"{killerName} killed {victimName}. \n";
        };
        this.UpdateRoomInfo(); // ���ӿ� ���������� �ѹ� �� ���� UI �ʱ�ȭ �������
        dof.active = false;
    }

    [PunRPC]
    public void UpdateKillLog(int killerNum, int victimNum)
    {
        var killerName = PhotonNetwork.CurrentRoom.Players[killerNum].NickName;
        var victimName = PhotonNetwork.CurrentRoom.Players[victimNum].NickName;
        this.log_Txt.text += $"{killerName} killed {victimName}. \n";
    }

    private void UpdateRoomInfo()
    {
        this.playerCntTxt.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";
        this.roomNameTxt.text = PhotonNetwork.CurrentRoom.Name;
    }
    /*public override void OnLeftRoom()
    {
        //SceneManager.LoadScene(0);
    }*/
    public override void OnPlayerEnteredRoom(Player newPlayer) // ������ �����ߴٸ�
    {
        this.UpdateRoomInfo();
        this.log_Txt.text += $"<color=#00ff00>{newPlayer.NickName}</color> is Joined Room\n";
    }
    public override void OnPlayerLeftRoom(Player otherPlayer) // ������ �����ߴٸ�
    {

        this.UpdateRoomInfo();

        this.log_Txt.text += $"<color=#ff0000>{otherPlayer.NickName}</color> is left Room\n";

    }
}
