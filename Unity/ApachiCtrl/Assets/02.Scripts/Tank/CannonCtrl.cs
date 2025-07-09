using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonCtrl : MonoBehaviour
{
    private TankInput input;
    public float rotationSpeed = 10f; // ȸ�� �ӵ�

    // ȸ�� ������ �����ϱ� ���� ����
    private float maxRotation = -38f; // ���� ���� ���� ��
    private float minRotation = 9f; // ���� �Ʒ��� ���� ��
    private float currentRotation = 0f; // ���� ȸ�� ����

    void Start()
    {
        this.input = GetComponentInParent<TankInput>();
    }
    void Update()
    {
        var rotate =-this.input.m_scroll * this.rotationSpeed;
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
