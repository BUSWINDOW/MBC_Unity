using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MariaCtrl : MonoBehaviour
{
    public enum ePlayerState
    {
        Idle, Attack, Under_Attack, Hit, Dead
    }
    ePlayerState state = ePlayerState.Idle;

    [Header("�ӵ� ����")]
    [Tooltip("�ȱ�")] public float walkSpeed = 5;
    [Tooltip("�޸���")] public float runSpeed = 10;
    [Header("ī�޶� ����")]
    [SerializeField] private Transform cameraTr;
    [SerializeField] private Transform cameraPivotTr;
    [SerializeField] private float cameraDist = 5.0f; // ī�޶�� �÷��̾� ���� �Ÿ�
    [Header("���콺 ����")]
    public Vector3 mouseMove = Vector3.zero; // ���콺 �̵��ϸ� �� ��ǥ 

    [Header("�÷��̾� ������ ����")]
    //[SerializeField] private Animator anim;
    private MariaAnimCtrl animCtrl;
    private MariaInput input;
    [SerializeField] private CharacterController ctrl;
    [SerializeField] private Vector3 moveVelocity = Vector3.zero;
    private Transform modelTr;
    private int playerLayer;
    private bool characterRotate = true;



    private bool isRun;
    public bool IsRun
    {
        get
        {
            return isRun;
        }
        set
        {
            isRun = value;
            //this.anim.SetBool("IsRun", isRun); // ������Ƽ�� ���鼭 �ڵ����� �ִϸ����� ����
            this.animCtrl.SetRun(isRun);
        }
    }
    private bool isGrounded;
    public bool IsGrounded
    {
        get
        {
            return isGrounded;
        }
        set
        {
            isGrounded = value;
            //this.anim.SetBool("IsGrounded", isGrounded); // ������Ƽ�� ���鼭 �ڵ����� �ִϸ����� ����
            this.animCtrl.SetGrounded(isGrounded);
        }
    }
    private float nextTime = 0; // ���� ���� �ð�
    void Start()
    {
        this.cameraTr = Camera.main.transform;
        this.cameraPivotTr = this.cameraTr.parent;
        playerLayer = LayerMask.NameToLayer("Player");
        this.modelTr = GetComponentInChildren<Transform>();
        this.ctrl = GetComponent<CharacterController>();
        this.animCtrl = GetComponent<MariaAnimCtrl>();
        this.cameraDist = 5;
        this.cameraTr.localPosition = new Vector3(0, 0, this.cameraDist);
    }

    void Update()
    {

        switch (state)
        {
            case ePlayerState.Idle:
                {
                    this.PlayerIdleAndMove();
                    break;
                }
            case ePlayerState.Attack:
                {
                    this.AttackTimeState();
                    break;
                }
            case ePlayerState.Under_Attack:
                {
                    this.AttackTimeState();
                    break;
                }
            case ePlayerState.Hit:
                {
                    break;
                }
            case ePlayerState.Dead:
                {
                    break;
                }
        }
        this.CameraDistanceCtrl();

    }
    private void LateUpdate() // �÷��̾ �����̰� �� �� ī�޶�
                              // �����̱� ���� LateUpdate
    {
        float cameraHeight = 1.3f; // ī�޶� ���� ���� : �÷��̾��� ���� ����
        this.cameraPivotTr.localPosition = this.transform.position + (Vector3.up * cameraHeight);
        this.mouseMove += new Vector3(-this.input.GetMouseY() * 10
                                     , this.input.GetMouseX() * 10,
                                        0f);

        if (mouseMove.x < -40)//���콺 ���� �̵� ����
            mouseMove.x = -40;
        else if (mouseMove.x > 40)
        {
            mouseMove.x = 40;
        }

        this.cameraPivotTr.localEulerAngles = mouseMove; // ī�޶��� �θ� ȸ�� ��Ŵ

        //ī�޶� ��ֹ��� �������� �ʵ��� ī�޶� ��ġ ����
        RaycastHit hit;
        Vector3 dir = (this.cameraTr.position - cameraPivotTr.position).normalized;
        if (Physics.Raycast(cameraPivotTr.position, dir, out hit, cameraDist, ~(1 << this.playerLayer))) // ī�޶��ʿ��� ī�޶� �Ǻ������� ���̽���
                                                                                                         // �÷��̾ �ƴѰ� �´´ٸ�
        {
            cameraTr.localPosition = Vector3.back * hit.distance; // ���� ��ġ �ٷ� �ڷ� ī�޶� �̵���Ų��
        }
        else
        {
            cameraTr.localPosition = Vector3.back * this.cameraDist;
        }
    }
    void PlayerIdleAndMove()
    {
        this.RunCheck();
        if (this.ctrl.isGrounded) // ĳ���� ��Ʈ�ѷ����� ��Ȯ�ϰ� üũ �����༭
                                  // ���� ���� �� ��Ȯ�ϰ� üũ
        {
            if (this.IsGrounded == false) this.IsGrounded = true;
            //else this.IsGrounded = false;

            this.CalcInputMove();
            RaycastHit groundHit; //���� üũ�ϴ� ����ĳ��Ʈ
            if (this.GroundCheck(out groundHit))
            {
                moveVelocity.y = IsRun ? -runSpeed : -walkSpeed;
            }
            else
            {
                moveVelocity.y = -1f;
            }

            this.PlayerAttack(); // ���� ����������� ����
        }
        else
        {
            if (this.IsGrounded == true) this.IsGrounded = false;
            //else this.IsGrounded= false;

            moveVelocity += Physics.gravity * Time.deltaTime; // �߷� ����
        }

        this.ctrl.Move(moveVelocity * Time.deltaTime); // ĳ���� ��Ʈ�ѷ� �̵�
    }

    void CalcInputMove()
    {
        moveVelocity = new Vector3(
            this.input.GetHorizontalRaw(),
            0f,
            this.input.GetVerticalRaw()).normalized * (IsRun ? runSpeed : walkSpeed);
        // �Է¿� ���� �̵� ���� ���
        //this.anim.SetFloat("SpeedX", Input.GetAxis("Horizontal")); // �ִϸ����Ϳ��� X�� �ӵ� ����
        this.animCtrl.SetSpeedX(this.input.GetHorizontal());
        //this.anim.SetFloat("SpeedY", ); // �ִϸ����Ϳ��� Y��(Z��) �ӵ� ����
        this.animCtrl.SetSpeedY(this.input.GetVertical());


        //�̵������� ���� �������� ���� �ʰ� ���� �������� ����
        this.moveVelocity = this.transform.TransformDirection(moveVelocity); // �÷��̾� �������� �̵����� ��ȯ(������ǥ �����)


        //�̵��� ī�޶� ȸ�� ����
        //RotateRistraint();


        if (this.characterRotate)
        {
            // ī�޶��� y�� ȸ���� ����
            float cameraYRotation = cameraPivotTr.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0f, cameraYRotation, 0f);

            // y�ุ ����� ȸ������ ������ ����
            modelTr.rotation = Quaternion.Slerp(modelTr.rotation, targetRotation, Time.deltaTime * 10f);
        }
        if (Input.GetMouseButtonDown(2))
        {
            this.characterRotate = !this.characterRotate;
        }


    }

    private void RotateRistraint()
    {
        if (this.moveVelocity.sqrMagnitude > 0.1f) // �̵��ӵ��� ���� �̻��� ��
        {
            Quaternion cameraRot = this.cameraPivotTr.rotation;
            cameraRot.x = cameraRot.z = 0f;
            this.cameraPivotTr.rotation = cameraRot;
            if (IsRun)
            {
                Quaternion characterRot = Quaternion.LookRotation(moveVelocity); // �̵��������� �÷��̾� �� ȸ��
                characterRot.x = characterRot.z = 0f;
                this.modelTr.rotation = Quaternion.Slerp(this.modelTr.rotation, characterRot, Time.deltaTime * 10);
            }
            else
            {
                modelTr.rotation = Quaternion.Slerp(this.modelTr.rotation,
                    cameraRot, Time.deltaTime * 10f);
            }
        }
    }


    void PlayerAttack()
    {
        if (!(this.state == ePlayerState.Idle)) return;
        if (this.input.GetFire1())
        {
            this.state = ePlayerState.Attack;
            //this.anim.SetTrigger("SwordAttack");
            this.animCtrl.SetAttack1();
            this.animCtrl.SetSpeedX(0);

            this.animCtrl.SetSpeedY(0);
            this.nextTime = 0;
        }
        else if (this.input.GetFire2())
        {
            this.state = ePlayerState.Under_Attack;
            //this.anim.SetTrigger("ShieldAttack");
            this.animCtrl.SetAttack2();

            this.animCtrl.SetSpeedX(0);

            this.animCtrl.SetSpeedY(0);
            this.nextTime = 0;
        }
    }

    void AttackTimeState()
    {
        this.nextTime += Time.deltaTime;
        if (nextTime >= 1)
        {
            this.state = ePlayerState.Idle;
        }
    }

    void RunCheck()
    {
        if (IsRun == false && Input.GetKey(KeyCode.LeftShift)) //����Ʈ ������ �޸�
        {
            this.IsRun = true;
        }
        else if (this.IsRun == true && this.input.GetHorizontal() == 0 && this.input.GetVertical() == 0) // �̵��� ���ϸ� �ȱ�� �ٲ�
                                                                                                           // ?
        {
            this.IsRun = false;
        }
    }
    bool GroundCheck(out RaycastHit hit)
    {
        return Physics.Raycast(this.transform.position, Vector3.down, out hit, 0.2f);
    }
    void CameraDistanceCtrl()
    {
        cameraDist -= this.input.GetMouseScroll() * 2f; // ���콺 �ٷ� ī�޶� �Ÿ� ����
        this.cameraDist = Mathf.Clamp(this.cameraDist, 1, 7);
    }
    void FreezeXZ()
    {
        this.transform.eulerAngles = new Vector3(0f, this.transform.eulerAngles.y, 0f);//x�� y�� ȸ�� ����
    }
}
