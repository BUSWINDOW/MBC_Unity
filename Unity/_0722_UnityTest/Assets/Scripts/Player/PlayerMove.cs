using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    PlayerInputCtrl inputCtrl;
    PlayerAnimCtrl animCtrl;
    Rigidbody rb;
    bool isJumping = false;

    CinemachineBrain mainCam;
    CinemachineVirtualCamera activeCam;



    public float jumpPower = 5;
    public float moveSpeed = 5;
    void Start()
    {
        this.inputCtrl = GetComponent<PlayerInputCtrl>();
        this.animCtrl = GetComponentInChildren<PlayerAnimCtrl>();
        this.rb = GetComponent<Rigidbody>();
        this.mainCam = Camera.main.GetComponent<CinemachineBrain>();
        this.activeCam = (CinemachineVirtualCamera)this.mainCam.ActiveVirtualCamera;
    }
    void Update()
    {
        Moving();
        Jumping();
        Rotate();
        this.activeCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y += this.inputCtrl.LookY * 0.1f;
        this.activeCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y = 
            Mathf.Clamp(this.activeCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y, -2, 5);
    }

    private void Rotate()
    {
        this.transform.Rotate(0, this.inputCtrl.Look, 0);
    }

    private void Jumping()
    {
        if (this.inputCtrl.Jump && !this.isJumping)
        {
            this.isJumping = true;
            this.rb.AddForce(Vector3.up * this.jumpPower, ForceMode.Impulse);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (this.isJumping)
        {
            this.isJumping = false;
            this.inputCtrl.Jump = false;
        }
    }

    private void Moving()
    {
        if (this.inputCtrl.moveDir.magnitude > 0.1f)
        {
            var dir = new Vector3(this.inputCtrl.moveDir.x, 0, this.inputCtrl.moveDir.y);
            this.animCtrl.transform.localRotation = Quaternion.LookRotation(dir);
            this.transform.Translate(new Vector3(this.inputCtrl.moveDir.x,0,this.inputCtrl.moveDir.y) * Time.deltaTime * this.moveSpeed);
        }
    }

}
