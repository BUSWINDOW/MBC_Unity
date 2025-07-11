using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUserID : MonoBehaviourPun
{
    public Text idText;
    void Start()
    {
        /*if (photonView.IsMine)
        {
            this.idText.text = photonView.Owner.NickName; // 로컬 플레이어의 닉네임을 표시
        }
        else
        {
            this.idText.text = "상대"; // 리모트 플레이어의 닉네임을 표시
        }*/
        if (photonView != null)
        {
            idText.text = photonView.Owner.NickName; // 포톤 뷰의 소유자의 닉네임을 표시
        }
    }
}
