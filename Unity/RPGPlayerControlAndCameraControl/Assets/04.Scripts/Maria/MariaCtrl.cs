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

    [Header("속도 설정")]
    [Tooltip("걷기")] public float walkSpeed = 5;
    [Tooltip("달리기")] public float runSpeed = 10;
    [Header("카메라 설정")]
    [SerializeField] private Transform cameraTr;
    [SerializeField] private Transform cameraPivotTr;
    [SerializeField] private float cameraDist = 5.0f; // 카메라와 플레이어 사이 거리
    [Header("마우스 설정")]
    public Vector3 mouseMove = Vector3.zero; // 마우스 이동하면 그 좌표 

    [Header("플레이어 움직임 설정")]
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
            //this.anim.SetBool("IsRun", isRun); // 프로퍼티를 쓰면서 자동으로 애니메이터 셋팅
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
            //this.anim.SetBool("IsGrounded", isGrounded); // 프로퍼티를 쓰면서 자동으로 애니메이터 셋팅
            this.animCtrl.SetGrounded(isGrounded);
        }
    }
    private float nextTime = 0; // 다음 공격 시간
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
    private void LateUpdate() // 플레이어가 움직이고 난 뒤 카메라를
                              // 움직이기 위한 LateUpdate
    {
        float cameraHeight = 1.3f; // 카메라 높이 설정 : 플레이어의 가슴 높이
        this.cameraPivotTr.localPosition = this.transform.position + (Vector3.up * cameraHeight);
        this.mouseMove += new Vector3(-this.input.GetMouseY() * 10
                                     , this.input.GetMouseX() * 10,
                                        0f);

        if (mouseMove.x < -40)//마우스 상하 이동 제한
            mouseMove.x = -40;
        else if (mouseMove.x > 40)
        {
            mouseMove.x = 40;
        }

        this.cameraPivotTr.localEulerAngles = mouseMove; // 카메라의 부모를 회전 시킴

        //카메라가 장애물에 가려지지 않도록 카메라 위치 조정
        RaycastHit hit;
        Vector3 dir = (this.cameraTr.position - cameraPivotTr.position).normalized;
        if (Physics.Raycast(cameraPivotTr.position, dir, out hit, cameraDist, ~(1 << this.playerLayer))) // 카메라쪽에서 카메라 피봇쪽으로 레이쏴서
                                                                                                         // 플레이어가 아닌게 맞는다면
        {
            cameraTr.localPosition = Vector3.back * hit.distance; // 맞은 위치 바로 뒤로 카메라를 이동시킨다
        }
        else
        {
            cameraTr.localPosition = Vector3.back * this.cameraDist;
        }
    }
    void PlayerIdleAndMove()
    {
        this.RunCheck();
        if (this.ctrl.isGrounded) // 캐릭터 컨트롤러에서 정확하게 체크 안해줘서
                                  // 따로 만들어서 더 정확하게 체크
        {
            if (this.IsGrounded == false) this.IsGrounded = true;
            //else this.IsGrounded = false;

            this.CalcInputMove();
            RaycastHit groundHit; //땅을 체크하는 레이캐스트
            if (this.GroundCheck(out groundHit))
            {
                moveVelocity.y = IsRun ? -runSpeed : -walkSpeed;
            }
            else
            {
                moveVelocity.y = -1f;
            }

            this.PlayerAttack(); // 땅에 닿아있을때만 공격
        }
        else
        {
            if (this.IsGrounded == true) this.IsGrounded = false;
            //else this.IsGrounded= false;

            moveVelocity += Physics.gravity * Time.deltaTime; // 중력 적용
        }

        this.ctrl.Move(moveVelocity * Time.deltaTime); // 캐릭터 컨트롤러 이동
    }

    void CalcInputMove()
    {
        moveVelocity = new Vector3(
            this.input.GetHorizontalRaw(),
            0f,
            this.input.GetVerticalRaw()).normalized * (IsRun ? runSpeed : walkSpeed);
        // 입력에 따른 이동 벡터 계산
        //this.anim.SetFloat("SpeedX", Input.GetAxis("Horizontal")); // 애니메이터에서 X축 속도 설정
        this.animCtrl.SetSpeedX(this.input.GetHorizontal());
        //this.anim.SetFloat("SpeedY", ); // 애니메이터에서 Y축(Z축) 속도 설정
        this.animCtrl.SetSpeedY(this.input.GetVertical());


        //이동방향을 로컬 방향으로 하지 않고 월드 방향으로 설정
        this.moveVelocity = this.transform.TransformDirection(moveVelocity); // 플레이어 방향으로 이동벡터 변환(월드좌표 기반임)


        //이동중 카메라 회전 제한
        //RotateRistraint();


        if (this.characterRotate)
        {
            // 카메라의 y축 회전만 따옴
            float cameraYRotation = cameraPivotTr.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0f, cameraYRotation, 0f);

            // y축만 사용한 회전으로 슬러프 적용
            modelTr.rotation = Quaternion.Slerp(modelTr.rotation, targetRotation, Time.deltaTime * 10f);
        }
        if (Input.GetMouseButtonDown(2))
        {
            this.characterRotate = !this.characterRotate;
        }


    }

    private void RotateRistraint()
    {
        if (this.moveVelocity.sqrMagnitude > 0.1f) // 이동속도가 일정 이상일 때
        {
            Quaternion cameraRot = this.cameraPivotTr.rotation;
            cameraRot.x = cameraRot.z = 0f;
            this.cameraPivotTr.rotation = cameraRot;
            if (IsRun)
            {
                Quaternion characterRot = Quaternion.LookRotation(moveVelocity); // 이동방향으로 플레이어 모델 회전
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
        if (IsRun == false && Input.GetKey(KeyCode.LeftShift)) //시프트 누르면 달림
        {
            this.IsRun = true;
        }
        else if (this.IsRun == true && this.input.GetHorizontal() == 0 && this.input.GetVertical() == 0) // 이동을 안하면 걷기로 바꿈
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
        cameraDist -= this.input.GetMouseScroll() * 2f; // 마우스 휠로 카메라 거리 조정
        this.cameraDist = Mathf.Clamp(this.cameraDist, 1, 7);
    }
    void FreezeXZ()
    {
        this.transform.eulerAngles = new Vector3(0f, this.transform.eulerAngles.y, 0f);//x축 y축 회전 제한
    }
}
