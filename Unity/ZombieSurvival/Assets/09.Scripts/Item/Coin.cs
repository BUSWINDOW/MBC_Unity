using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Coin : MonoBehaviourPun, IItem
{
    public int score = 1000;

    public void Use(GameObject target)
    {
        var player = target.GetComponent<WomanHealth>();
        if (player != null)
        {
            GameManager.Instance.AddScore(score);
        }
        //Destroy(gameObject); // ������ ��� �� ����
        PhotonNetwork.Destroy(gameObject); // PhotonNetwork�� ���� ������ ����
    }
}
