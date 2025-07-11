using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class TankMove : MonoBehaviourPun , IPunObservable
{
    public TankInput input;
    public Rigidbody rb;

    public float speed = 10f; // ��ũ�� �̵� �ӵ�

    Vector3 curPos = Vector3.zero; // �ٸ� ��Ʈ��ũ�� �̵���
    Quaternion curRot = Quaternion.identity; // �ٸ� ��Ʈ��ũ�� ȸ����

    IEnumerator Start()
    {
        this.input = GetComponent<TankInput>();
        this.rb = GetComponent<Rigidbody>();
        this.rb.centerOfMass = new Vector3(0,-0.5f,0); // ���� �������� ��ũ�� �߽��� ����

        this.curPos = this.transform.position; // ���� ��ġ�� ����
        this.curRot = this.transform.rotation; // ���� ȸ���� ����

        yield return null;


        if (photonView.IsMine) // ���� ��Ʈ��ũ �� ���� �䰡 ���� ���̶��(���� �̶��)
        {
            CinemachineVirtualCamera vCam = FindObjectOfType<CinemachineVirtualCamera>();
            if (vCam != null)
            {
                vCam.Follow = this.transform; // ���� ī�޶� ��ũ�� ���󰡵��� ����
                vCam.LookAt = this.transform; // ���� ī�޶� ��ũ�� �ٶ󺸵��� ����
            }
        }
    }

    //������ �̵� ȸ���� ��Ʈ��ũ���� Ÿ���� ����Ʈ�� �۽��ϰ�,
    //�ݴ�� ����Ʈ�� �̵� ȸ���� ���Ź޾Ƽ� ��Ʈ��ũ �� �������� ���� ������ �ϱ� ������
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) //�۽����̶��
        {
            stream.SendNext(this.transform.position);
            stream.SendNext(this.transform.rotation);
        }
        else if (stream.IsReading) //�������̶��
                                   //��� �׳� �۽����� �ƴ϶�� �̶� ������ else�� �ص� �ȴ�
        {
            this.curPos = (Vector3)stream.ReceiveNext(); // �۽ŵ� ��ġ���� �޴´�
            this.curRot = (Quaternion)stream.ReceiveNext(); // �۽ŵ� ȸ������ �޴´�
        }
    }
    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            #region ��ũ ȸ��
            this.transform.Rotate(0, input.h * 2f, 0, Space.Self);
            #endregion

            #region ��ũ �̵�
            this.transform.Translate(Vector3.forward * this.input.v * Time.deltaTime * this.speed);
            #endregion
        }
        else
        {
            this.transform.position = Vector3.Lerp(this.transform.position, this.curPos, Time.deltaTime * 3f); // �ٸ� ��Ʈ��ũ�� ��ġ������ ����
            //this.transform.position = this.curPos;
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this.curRot, Time.deltaTime * 3f); // �ٸ� ��Ʈ��ũ�� ȸ�������� ����
            //this.transform.rotation = this.curRot;
        }

    }
}
