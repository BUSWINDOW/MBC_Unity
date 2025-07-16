using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AmmoPack : MonoBehaviourPun , IItem
{
    public int ammo = 10;
    // Implement the Use method from IItem interface


    public void Use(GameObject target)
    {

        var shooter = target.GetComponent<WomanShooter>();
        if (shooter != null && shooter.gun != null)
        {
            shooter.gun.ammo += ammo; // 총기의 탄약을 증가시킴
            //기존엔 gun 내부에 함수가 따로있고, 그걸 PunRPC로 만들고, 그걸 photonView.RPC로 호출
            // 테스트로 바꿈
        }
        //Destroy(gameObject); // 아이템 사용 후 제거
        PhotonNetwork.Destroy(gameObject); // PhotonNetwork를 통해 아이템 제거
    }
}
