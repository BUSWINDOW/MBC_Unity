using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApacheCtrl : MonoBehaviour
{
    public Rigidbody rb; // Rigidbody ������Ʈ ���� ����
    public float moveSpeed = 0f; // ���� �ӵ� ���� ����
    public float rotateSpeed = 0f; // ȸ�� �ӵ� ���� ����
    public float verticalSpeed = 0f; // ���� �ӵ� ���� ����

    float curRot = 0;
    void Start()
    {
    }
    void FixedUpdate() // Ű���� �Է°���ŭ �̵��ϱ� ���� ����
    {
        #region �¿�� ȸ�� �ϴ� ����
        if (Input.GetKey(KeyCode.A))
        {
            this.rotateSpeed -= 0.02f; // ���� ����Ű�� ������ �������� ȸ��
        }
        else if (Input.GetKey(KeyCode.D))
        {
            this.rotateSpeed += 0.02f; // ������ ����Ű�� ������ ���������� ȸ��
        }
        else if (this.rotateSpeed != 0)
        {
            this.rotateSpeed += this.rotateSpeed != 0 ? this.rotateSpeed > 0f ? -0.02f : 0.02f : 0; // ȸ�� �ӵ��� 0�� �ǵ��� ����
        }
        this.transform.Rotate(0f, this.rotateSpeed, 0f); // ���� ȸ�� �ӵ���ŭ ȸ��
        #endregion

        #region �յڷ� �̵� �ϴ� ����
        if (Input.GetKey(KeyCode.W))
        {
            this.moveSpeed += 0.02f; // ���� ����Ű�� ������ ������ �̵�
        }
        else if (Input.GetKey(KeyCode.S))
        {
            this.moveSpeed -= 0.02f; // ���� ����Ű�� ������ �ڷ� �̵�
        }
        else if (this.moveSpeed != 0)
        {
            this.moveSpeed += this.moveSpeed != 0 ? this.moveSpeed > 0f ? -0.02f : 0.02f : 0;
        }
        this.transform.Translate(Vector3.forward * this.moveSpeed * Time.deltaTime, Space.Self);
        #endregion

        #region �������� �̵� �ϴ� ����
        if (Input.GetKey(KeyCode.Space))
        {
            this.verticalSpeed += 0.02f; // �����̽��ٸ� ������ ���߿� ��
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            this.verticalSpeed -= 0.02f; // alt������ ����
        }
        else if (this.verticalSpeed != 0)
        {
            this.verticalSpeed += this.verticalSpeed != 0 ? this.verticalSpeed > 0f ? -0.02f : 0.02f : 0;
        }
        this.transform.Translate(Vector3.up * this.verticalSpeed * Time.deltaTime, Space.World);
        #endregion

        #region ���콺 �ٷ� ���� �����ϴ� ����
        var rotate = Input.GetAxisRaw("Mouse ScrollWheel") * 10; // ���콺 �� �Է°��� ������
        
        curRot += rotate;

        this.transform.rotation = Quaternion.Euler(curRot, this.transform.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z);

        #endregion

    }
}