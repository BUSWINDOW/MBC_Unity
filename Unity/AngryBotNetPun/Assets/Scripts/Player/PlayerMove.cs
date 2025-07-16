using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    CharacterController controller;
    PlayerInput input;
    Animator anim;
    Camera cam;
    public float moveSpeed = 10;
    public float turnSpeed = 90;
    private Ray ray;
    private readonly int hashForward = Animator.StringToHash("Forward");
    private readonly int hashStrafe = Animator.StringToHash("Strafe");

    void Start()
    {
        this.controller = GetComponent<CharacterController>();
        this.input = GetComponent<PlayerInput>();
        this.anim = GetComponent<Animator>();
        this.cam = Camera.main;
    }


    void Update()
    {
        this.Move();
        this.Turn();
    }
    void Move()
    {
        Vector3 cameraForward = cam.transform.forward;
        Vector3 cameraRight = cam.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;

        //�̵��� ���� ���
        Vector3 moveDir = cameraForward * input.v + cameraRight * input.h;
        moveDir.Set(moveDir.x, 0, moveDir.z);

        // ���ΰ� ĳ���� �̵� ó��, ĳ���� ��Ʈ�ѷ��� �̵�
        this.controller.SimpleMove(moveDir * moveSpeed);

        // �ִϸ��̼� ó��
        float forward = Vector3.Dot(moveDir, this.transform.forward);
        float strafe = Vector3.Dot(moveDir, this.transform.right);
        this.anim.SetFloat(hashForward, forward);
        this.anim.SetFloat(hashStrafe, strafe);
    }
    void Turn()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        plane.Raycast(ray, out float enter);
        var hitPoint = ray.GetPoint(enter);
        Vector3 dir = hitPoint - this.transform.position;
        dir.y = 0; // y�� ȸ���� ����
        if (dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }
}
