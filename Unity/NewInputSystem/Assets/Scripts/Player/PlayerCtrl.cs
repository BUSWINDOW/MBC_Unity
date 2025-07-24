//#pragma warning disable IDE0051
//��� ���� ��Ȱ��ȭ�ϴ� ����
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerCtrl : MonoBehaviour
{

    public Vector3 moveDir;
    public float moveSpeed = 2;

    private void Start()
    {

    }
    private void Update()
    {
        if(this.moveDir != Vector3.zero)
        {
            this.transform.rotation = Quaternion.LookRotation(this.moveDir);
            this.transform.Translate(Vector3.forward * Time.deltaTime * this.moveSpeed);
        }
    }
}
