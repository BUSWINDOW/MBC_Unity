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
            shooter.gun.ammo += ammo; // �ѱ��� ź���� ������Ŵ
            //������ gun ���ο� �Լ��� �����ְ�, �װ� PunRPC�� �����, �װ� photonView.RPC�� ȣ��
            // �׽�Ʈ�� �ٲ�
        }
        //Destroy(gameObject); // ������ ��� �� ����
        PhotonNetwork.Destroy(gameObject); // PhotonNetwork�� ���� ������ ����
    }
}
