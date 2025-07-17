using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerMove : MonoBehaviourPun/*,IPunObservable*/
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
    Plane plane;

    Vector3 receivePos = Vector3.zero;
    Quaternion receiveRot = Quaternion.identity;

    void Start()
    {
        this.controller = GetComponent<CharacterController>();
        this.input = GetComponent<PlayerInput>();
        this.anim = GetComponent<Animator>();
        this.cam = Camera.main;
        this.plane = new Plane(Vector3.up, transform.position);
    }


    void Update()
    {
        if (photonView.IsMine)
        {
            this.Move();
            this.Turn();
        }
        /*else
        {
            this.transform.position = this.receivePos;
            this.transform.rotation = this.receiveRot;
        }*/

    }

/*    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this.transform.position);
            stream.SendNext(this.transform.rotation);
        }
        else
        {
            this.receivePos = (Vector3)stream.ReceiveNext();
            this.receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }*/

    void Move()
    {
        Vector3 cameraForward = cam.transform.forward;
        Vector3 cameraRight = cam.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;

        //이동할 벡터 계산
        Vector3 moveDir = cameraForward * input.v + cameraRight * input.h;
        moveDir.Set(moveDir.x, 0, moveDir.z);

        // 주인공 캐릭터 이동 처리, 캐릭터 컨트롤러로 이동
        this.controller.SimpleMove(moveDir * moveSpeed);

        // 애니메이션 처리
        float forward = Vector3.Dot(moveDir, this.transform.forward);
        float strafe = Vector3.Dot(moveDir, this.transform.right);
        this.anim.SetFloat(hashForward, forward);
        this.anim.SetFloat(hashStrafe, strafe);
    }
    void Turn()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        plane.Raycast(ray, out float enter);
        var hitPoint = ray.GetPoint(enter);
        Vector3 dir = hitPoint - this.transform.position;
        dir.y = 0; // y축 회전만 적용
        if (dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            //this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            this.transform.rotation = targetRotation;
        }
    }


}
