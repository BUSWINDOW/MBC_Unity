using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomData : MonoBehaviourPun
{
    private RoomInfo _info;

    public TMP_Text roomNameText;
    public Button roomBtn;

    public RoomInfo RoomInfo 
    {
        get 
        {
            return _info;
            
        }
        set 
        {
            _info = value;
            roomNameText.text = $"{_info.Name}\n({_info.PlayerCount}/{_info.MaxPlayers})";
        }
    }
}
