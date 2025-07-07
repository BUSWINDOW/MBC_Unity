using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WomanMovement : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rb;
    private WomanInput input;

    [Header("�÷��̾� ����")]
    public float moveSpeed = 5f; // �̵� �ӵ�
    public float rotSpeed = 180f; // ȸ�� �ӵ�

    private readonly string moveParam = "Move"; // �ִϸ��̼� �Ķ���� �̸�

    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
        this.anim = GetComponent<Animator>();
        this.input = GetComponent<WomanInput>();
    }
    void FixedUpdate() // ���� �̵� �κ�
    {
         this.Move();
        this.Rotate();
    }
    void Move()
    {
        Vector3 moveDistance = this.input.Move * transform.forward * this.moveSpeed * Time.fixedDeltaTime;
        this.anim.SetFloat(this.moveParam, this.input.Move);
        this.rb.MovePosition(this.rb.position + moveDistance);
    }
    void Rotate()
    {
        float turn = this.input.Rotate * this.rotSpeed * Time.fixedDeltaTime; // 180�� ȸ��
        this.rb.rotation = this.rb.rotation * Quaternion.Euler(0f, turn, 0f); // Quaternion.Euler(0f, turn, 0f) : Y���� �������� ȸ��
    }
}
