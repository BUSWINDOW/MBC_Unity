using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


//���콺 x�� ȸ���� ���缭 ȸ����

public class TurretCtrl : MonoBehaviourPun, IPunObservable
{
    Ray ray;
    RaycastHit hit;

    Quaternion curRot = Quaternion.identity; // ���� ȸ����

    void Start()
    {
        this.curRot = this.transform.rotation; // ���� ȸ���� ����
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this.transform.rotation);
        }
        else
        {
            this.curRot = (Quaternion)stream.ReceiveNext(); // �۽ŵ� ȸ������ �޴´�
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            // ���� �÷��̾��� ��� ���콺 �Է¿� ���� ȸ��
            Rotate();
        }
        else
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, curRot, Time.deltaTime * 10f);
        }
    }

    private void Rotate()
    {
        this.ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(this.ray.origin, this.ray.direction * 100f, Color.red);
        if (Physics.Raycast(this.ray, out this.hit, 100f, 1 << 6))
        {
            Vector3 target = this.hit.point;
            target.y = this.transform.position.y; // y���� ����
            Vector3 direction = target - this.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, Time.deltaTime * 10f);
        }
    }
}
