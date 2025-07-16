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
        //Destroy(gameObject); // 아이템 사용 후 제거
        PhotonNetwork.Destroy(gameObject); // PhotonNetwork를 통해 아이템 제거
    }
}
