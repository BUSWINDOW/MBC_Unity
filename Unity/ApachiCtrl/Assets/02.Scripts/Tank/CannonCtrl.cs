using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CannonCtrl : MonoBehaviourPun, IPunObservable
{
    private TankInput input;
    public float rotationSpeed = 10f; // ȸ�� �ӵ�

    // ȸ�� ������ �����ϱ� ���� ����
    private float maxRotation = -38f; // ���� ���� ���� ��
    private float minRotation = 9f; // ���� �Ʒ��� ���� ��
    private float currentRotation = 0f; // ���� ȸ�� ����

    Quaternion curRot = Quaternion.identity; // ���� ȸ����

    void Start()
    {
        this.input = GetComponentInParent<TankInput>();
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
            Rotate();
        }
        else
        {
            // �ٸ� �÷��̾��� ȸ������ ����
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, curRot, Time.deltaTime * 10f);
        }
    }

    private void Rotate()
    {
        var rotate = -this.input.m_scroll * this.rotationSpeed;
        //this.transform.Rotate(Vector3.right * rotate);
        //ȸ�� ������ �����ؾ���
        currentRotation += rotate;
        //9~ -38 ���� ������

        if (currentRotation < maxRotation) //�������� ���� �ö�
                                           //x���� ���������� ������(���) �������ϱ� �������� ���� �ö�
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
