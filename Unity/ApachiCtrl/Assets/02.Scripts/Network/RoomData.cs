using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class RoomData : MonoBehaviourPun
{
    public string roomName = string.Empty;
    public int connectPlayers = 0;
    public int maxPlayers = 0;
    public Text roomNameText;
    public Text playerCntText;
    

    public void DisplayPlayerRoomData()
    {
        this.roomNameText.text = this.roomName;
        this.playerCntText.text = $"{this.connectPlayers}/{this.maxPlayers}"; // 플레이어 수 표시
    }

}
