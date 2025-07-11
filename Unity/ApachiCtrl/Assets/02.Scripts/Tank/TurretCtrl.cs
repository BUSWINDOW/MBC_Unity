using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


//마우스 x축 회전에 맞춰서 회전함

public class TurretCtrl : MonoBehaviourPun, IPunObservable
{
    Ray ray;
    RaycastHit hit;

    Quaternion curRot = Quaternion.identity; // 현재 회전값

    void Start()
    {
        this.curRot = this.transform.rotation; // 현재 회전을 저장
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this.transform.rotation);
        }
        else
        {
            this.curRot = (Quaternion)stream.ReceiveNext(); // 송신된 회전값을 받는다
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            // 로컬 플레이어의 경우 마우스 입력에 따라 회전
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
            target.y = this.transform.position.y; // y축은 고정
            Vector3 direction = target - this.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, Time.deltaTime * 10f);
        }
    }
}
