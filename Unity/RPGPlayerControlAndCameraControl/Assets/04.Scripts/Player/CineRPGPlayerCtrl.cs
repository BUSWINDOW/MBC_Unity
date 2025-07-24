using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Animations;

public class CineRPGPlayerCtrl : MonoBehaviour
{
    CineRPGPlayerInput inputCtrl;
    Rigidbody rb;
    bool isAttacking = false;
    CinemachineBrain mainCam;
    CinemachineVirtualCamera activeCam;

    CineRPGPlayerAnimCtrl model;

    public float jumpPower = 5;
    public float moveSpeed = 5;

    public bool charRotate = true;
    public bool CharRotate
    {
        get
        {
            return charRotate;
        }
        set
        {
            this.charRotate = value;
            this.cameraLookTr.GetComponent<RotationConstraint>().enabled = value;
            //this.cameraLookTr.GetComponent<RotationConstraint>().rotationAxis = Axis.X | Axis.Z;
        }
    }
    public Transform cameraLookTr;

    void Start()
    {
        this.inputCtrl = GetComponent<CineRPGPlayerInput>();
        this.model = GetComponentInChildren<CineRPGPlayerAnimCtrl>();
        this.mainCam = Camera.main.GetComponent<CinemachineBrain>();
        this.activeCam = (CinemachineVirtualCamera)this.mainCam.ActiveVirtualCamera;

        var a = new ConstraintSource();
        a.sourceTransform = this.transform;
        a.weight = 1;
        this.cameraLookTr.GetComponent<RotationConstraint>().AddSource(a);
        this.cameraLookTr.GetComponent<PositionConstraint>().AddSource(a);
    }
    void Update()
    {
        Moving();
        Attacking();
        Rotate();
        this.activeCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y -= this.inputCtrl.LookY * 0.1f;
        this.activeCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y =
            Mathf.Clamp(this.activeCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y, -2, 5);
        if (Input.GetMouseButtonDown(2))
        {
            this.CharRotate = !this.CharRotate;
        }
    }

    private void Rotate()
    {
        if (this.charRotate)
            this.transform.Rotate(0, this.inputCtrl.LookX, 0);
        else
        {
            this.cameraLookTr.Rotate(0, this.inputCtrl.LookX, 0);
        }
    }

    private void Attacking()
    {
        if (this.inputCtrl.Attack && !this.isAttacking)
        {
            this.isAttacking = true;
        }
    }


    private void Moving()
    {
        if (this.inputCtrl.MoveDir.magnitude > 0.1f)
        {
            var dir = new Vector3(this.inputCtrl.MoveDir.x, 0, this.inputCtrl.MoveDir.y);
            this.model.transform.localRotation = Quaternion.LookRotation(dir);
            this.transform.Translate(new Vector3(this.inputCtrl.MoveDir.x, 0, this.inputCtrl.MoveDir.y) * Time.deltaTime * this.moveSpeed);
        }
    }
}
