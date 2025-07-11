using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CannonCtrl : MonoBehaviourPun, IPunObservable
{
    private TankInput input;
    public float rotationSpeed = 10f; // 회전 속도

    // 회전 각도를 제한하기 위한 변수
    private float maxRotation = -38f; // 제일 위에 있을 때
    private float minRotation = 9f; // 제일 아래에 있을 때
    private float currentRotation = 0f; // 현재 회전 각도

    Quaternion curRot = Quaternion.identity; // 현재 회전값

    void Start()
    {
        this.input = GetComponentInParent<TankInput>();
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
            Rotate();
        }
        else
        {
            // 다른 플레이어의 회전값을 적용
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, curRot, Time.deltaTime * 10f);
        }
    }

    private void Rotate()
    {
        var rotate = -this.input.m_scroll * this.rotationSpeed;
        //this.transform.Rotate(Vector3.right * rotate);
        //회전 정도를 제한해야함
        currentRotation += rotate;
        //9~ -38 까지 움직임

        if (currentRotation < maxRotation) //음수여야 위로 올라감
                                           //x축을 오른쪽으로 돌리면(양수) 내려가니까 음수여야 위로 올라감
        {
            currentRotation = maxRotation;
        }
        else if (currentRotation > minRotation)
        {
            currentRotation = minRotation;
        }
        this.transform.rotation = Quaternion.Euler(currentRotation, this.transform.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z);
    }

}
