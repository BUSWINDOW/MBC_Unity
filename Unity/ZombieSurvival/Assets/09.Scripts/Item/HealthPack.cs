using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HealthPack : MonoBehaviourPun, IItem
{
    public int health = 50;


    public void Use(GameObject target)
    {

        var player = target.GetComponent<WomanHealth>();
        if (player != null)
        {
            player.RestoreHealth(health); // 플레이어의 체력 회복
        }
        //Destroy(gameObject); // 아이템 사용 후 제거
        PhotonNetwork.Destroy(gameObject); // PhotonNetwork를 통해 아이템 제거
    }
}
