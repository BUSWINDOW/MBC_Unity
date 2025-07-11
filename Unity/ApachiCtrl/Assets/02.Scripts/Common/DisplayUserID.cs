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
            this.idText.text = photonView.Owner.NickName; // ���� �÷��̾��� �г����� ǥ��
        }
        else
        {
            this.idText.text = "���"; // ����Ʈ �÷��̾��� �г����� ǥ��
        }*/
        if (photonView != null)
        {
            idText.text = photonView.Owner.NickName; // ���� ���� �������� �г����� ǥ��
        }
    }
}
