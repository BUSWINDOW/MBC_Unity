using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WomanMovement : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rb;
    private WomanInput input;

    [Header("플레이어 스탯")]
    public float moveSpeed = 5f; // 이동 속도
    public float rotSpeed = 180f; // 회전 속도

    private readonly string moveParam = "Move"; // 애니메이션 파라미터 이름

    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
        this.anim = GetComponent<Animator>();
        this.input = GetComponent<WomanInput>();
    }
    void FixedUpdate() // 실제 이동 부분
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
        float turn = this.input.Rotate * this.rotSpeed * Time.fixedDeltaTime; // 180도 회전
        this.rb.rotation = this.rb.rotation * Quaternion.Euler(0f, turn, 0f); // Quaternion.Euler(0f, turn, 0f) : Y축을 기준으로 회전
    }
}
