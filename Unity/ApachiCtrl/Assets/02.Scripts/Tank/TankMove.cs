using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class TankMove : MonoBehaviourPun , IPunObservable
{
    public TankInput input;
    public Rigidbody rb;

    public float speed = 10f; // 탱크의 이동 속도

    Vector3 curPos = Vector3.zero; // 다른 네트워크의 이동값
    Quaternion curRot = Quaternion.identity; // 다른 네트워크의 회전값

    IEnumerator Start()
    {
        this.input = GetComponent<TankInput>();
        this.rb = GetComponent<Rigidbody>();
        this.rb.centerOfMass = new Vector3(0,-0.5f,0); // 물리 엔진에서 탱크의 중심을 조정

        this.curPos = this.transform.position; // 현재 위치를 저장
        this.curRot = this.transform.rotation; // 현재 회전을 저장

        yield return null;


        if (photonView.IsMine) // 포톤 네트워크 상 포톤 뷰가 나의 것이라면(로컬 이라면)
        {
            CinemachineVirtualCamera vCam = FindObjectOfType<CinemachineVirtualCamera>();
            if (vCam != null)
            {
                vCam.Follow = this.transform; // 가상 카메라가 탱크를 따라가도록 설정
                vCam.LookAt = this.transform; // 가상 카메라가 탱크를 바라보도록 설정
            }
        }
    }

    //로컬의 이동 회전을 네트워크상의 타인인 리모트에 송신하고,
    //반대로 리모트의 이동 회전을 수신받아서 네트워크 상 움직임이 서로 보여야 하기 때문에
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) //송신중이라면
        {
            stream.SendNext(this.transform.position);
            stream.SendNext(this.transform.rotation);
        }
        else if (stream.IsReading) //수신중이라면
                                   //사실 그냥 송신중이 아니라면 이란 뜻으로 else만 해도 된다
        {
            this.curPos = (Vector3)stream.ReceiveNext(); // 송신된 위치값을 받는다
            this.curRot = (Quaternion)stream.ReceiveNext(); // 송신된 회전값을 받는다
        }
    }
    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            #region 탱크 회전
            this.transform.Rotate(0, input.h * 2f, 0, Space.Self);
            #endregion

            #region 탱크 이동
            this.transform.Translate(Vector3.forward * this.input.v * Time.deltaTime * this.speed);
            #endregion
        }
        else
        {
            this.transform.position = Vector3.Lerp(this.transform.position, this.curPos, Time.deltaTime * 3f); // 다른 네트워크의 위치값으로 보간
            //this.transform.position = this.curPos;
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this.curRot, Time.deltaTime * 3f); // 다른 네트워크의 회전값으로 보간
            //this.transform.rotation = this.curRot;
        }

    }
}
