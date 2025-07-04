using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//1.Wheel Colider <- 바닥 마찰로 이동
//2.Wheel Models <- 콜라이더랑 같이 회전
//

public class PlayerCar : MonoBehaviour
{
    /*public enum ePos
    {
        Front_Left, Front_Right, Back_Left, Back_Right
    }
    
    [SerializeField] public Dictionary<ePos, WheelCollider> WheelColliders;
    [SerializeField] public Dictionary<ePos, Transform> WheelModels;*/

    [Header("콜라이더")]
    public WheelCollider FL_c;
    public WheelCollider FR_c;
    public WheelCollider BL_c;
    public WheelCollider BR_c;

    [Header("모델")]
    public Transform FL_m;
    public Transform FR_m;
    public Transform BL_m;
    public Transform BR_m;

    [Header("무게 중심")]
    public Vector3 centerOfMass = new Vector3(0,-0.5f, 0); // 무게중심
    
    [Header("Car Angle")]
    public float maxSteerAngle = 35f; // 최대 조향각
     public float maxMotorTorque = 2500f; // 최대 토크(가속력)
     public float maxBrakeTorque = 3500f; // 브레이크 토크(감속력)
     public float maxSpeed = 200f; // 브레이크 토크(감속력)
    public float emergencyBrakeForce = 80000;

     [Header("Car Current Speed")] public float carCurrentSpeed = 0f; // 브레이크 토크(감속력)


    private Rigidbody rb;

    public float SteerInput = 0f; //ad키를 받는 회전
    public float motorInput; // 차량의 모터토크를 저장하기 위한 변수
    public bool isEmergencyBraking = false;


    private PlayerCarLight lightCtrl;
    private PlayerCarRide rideCtrl;
    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
        this.lightCtrl = GetComponent<PlayerCarLight>();
        this.rideCtrl = GetComponent<PlayerCarRide>();
        if(this.rb != null)
        {
            rb.centerOfMass = centerOfMass;
        }
    }
    private void Update() // 키입력 받는 곳
    {
        this.SteerInput = Input.GetAxis("Horizontal");
        //this.forward = Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1); // w키만 받기 위해 Clamp를 활용해서 0 이하로 안내려가게함
        //this.back = -1 * Mathf.Clamp(Input.GetAxis("Vertical"), -1, 0); // s키만 받기 위해 Clamp를 활용해서 0 이상으로 안올라가게함
        // -1을 곱해서 양수로 만들어서 계산을 용이하게 함



        this.motorInput = Input.GetAxis("Vertical");
        /*if (Input.GetKey(KeyCode.W)) // 전진중 일때
        {
            StartCoroutine(ForwardCar());
        }
        else if (Input.GetKey(KeyCode.S)) // 후진중 일때
        {
            StartCoroutine(BackwardCar());
        }*/

        this.isEmergencyBraking = Input.GetKey(KeyCode.LeftShift);
        this.carCurrentSpeed = rb.velocity.sqrMagnitude;
        if (Input.GetKeyDown(KeyCode.F))
        {
            this.lightCtrl.FlashTurnOn_Off();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            this.lightCtrl.FlashTurnOn_Off(false);
            this.lightCtrl.BackLightCtrl(false);
            this.enabled = false;
            this.rideCtrl.GetOffCar();
        }
    }
    void FixedUpdate()
    {



        /*//이륜구동 만드는거라 뒷바퀴만 회전시킬거임
        #region 뒷 바퀴 회전 X축(전방 후방)
        this.BL_c.motorTorque = this.maxMotorTorque * motorInput;
        this.BR_c.motorTorque = this.maxMotorTorque * motorInput;
        #endregion
        //앞바퀴는 좌우 회전 시킬거임
        #region 앞 바퀴 회전 Y축(좌우)
        this.FL_c.steerAngle = this.SteerInput * this.maxSteerAngle;
        this.FR_c.steerAngle = this.SteerInput * this.maxSteerAngle;
        #endregion
        //뒷바퀴에 브레이크걸면 멈춰짐
        #region 브레이크 걸기
        this.BL_c.brakeTorque = this.maxBrakeTorque * brakeInput;
        this.BR_c.brakeTorque = this.maxBrakeTorque * brakeInput;
        #endregion*/


        HandleMotor();


        
    }

    void HandleMotor()
    {
        //비상 브레이크를 밟을 시, 차의 모든 구동력을 통제해야함
        if (isEmergencyBraking)
        {
            //뒷바퀴 속도 0으로

            this.BL_c.motorTorque =0;
            this.BR_c.motorTorque =0;
                                  

            //뒷바퀴에 브레이크걸면 멈춰짐

            this.BL_c.brakeTorque = this.emergencyBrakeForce;
            this.BR_c.brakeTorque = this.emergencyBrakeForce;
            this.lightCtrl.BackLightCtrl(false);

        }
        else
        {
            //비상 브레이크가 아니라면, 일반적인 주행
            //이륜구동 만드는거라 뒷바퀴만 회전시킬거임
            #region 뒷 바퀴 회전 X축(전방 후방)
            this.BL_c.motorTorque = this.maxMotorTorque * motorInput;
            this.BR_c.motorTorque = this.maxMotorTorque * motorInput;
            #endregion
            //앞바퀴는 좌우 회전 시킬거임
            #region 앞 바퀴 회전 Y축(좌우)
            this.FL_c.steerAngle = this.SteerInput * this.maxSteerAngle;
            this.FR_c.steerAngle = this.SteerInput * this.maxSteerAngle;
            #endregion
            //뒷바퀴에 걸었던 브레이크 해제

            this.BL_c.brakeTorque = 0;
            this.BR_c.brakeTorque = 0;
            
            if(this.motorInput > 0)
            {
                this.lightCtrl.BackLightCtrl(true, Color.green);
            }
            else
            {
                this.lightCtrl.BackLightCtrl(true, Color.red);
            }
        }
    }

    private void LateUpdate()
    {
        #region 모델링도 회전 시키기
        /* this.FL_m.localEulerAngles = new Vector3(FL_c.transform.localEulerAngles.x, this.SteerInput * maxSteerAngle, FL_c.transform.localEulerAngles.z);
         this.FR_m.localEulerAngles = new Vector3(FR_c.transform.localEulerAngles.x, this.SteerInput * maxSteerAngle, FR_c.transform.localEulerAngles.z);
         // 앞바퀴는 y축도 회전 시켜야함

         FL_m.Rotate(this.BL_c.rpm * Time.fixedDeltaTime, 0, 0);
         FR_m.Rotate(this.BR_c.rpm * Time.fixedDeltaTime, 0, 0);
         BL_m.Rotate(this.BL_c.rpm * Time.fixedDeltaTime, 0, 0);
         BR_m.Rotate(this.BR_c.rpm * Time.fixedDeltaTime, 0, 0);
         //앞으로 구르는건 모든 바퀴 다 굴러야함
         //RPM = 분당 회전수*/
        #endregion
        this.UpdateWheelModel(this.FL_c,FL_m);
        this.UpdateWheelModel(this.FR_c,FR_m);
        this.UpdateWheelModel(this.BL_c,BL_m);
        this.UpdateWheelModel(this.BR_c,BR_m);
    }

    void UpdateWheelModel(WheelCollider col , Transform model)
    {
        Vector3 pos;
        Quaternion rot;

        col.GetWorldPose(out pos, out rot);
        model.position = pos;
        model.rotation = rot;
    }
}
