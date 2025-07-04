using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//1.Wheel Colider <- �ٴ� ������ �̵�
//2.Wheel Models <- �ݶ��̴��� ���� ȸ��
//

public class PlayerCar : MonoBehaviour
{
    /*public enum ePos
    {
        Front_Left, Front_Right, Back_Left, Back_Right
    }
    
    [SerializeField] public Dictionary<ePos, WheelCollider> WheelColliders;
    [SerializeField] public Dictionary<ePos, Transform> WheelModels;*/

    [Header("�ݶ��̴�")]
    public WheelCollider FL_c;
    public WheelCollider FR_c;
    public WheelCollider BL_c;
    public WheelCollider BR_c;

    [Header("��")]
    public Transform FL_m;
    public Transform FR_m;
    public Transform BL_m;
    public Transform BR_m;

    [Header("���� �߽�")]
    public Vector3 centerOfMass = new Vector3(0,-0.5f, 0); // �����߽�
    
    [Header("Car Angle")]
    public float maxSteerAngle = 35f; // �ִ� ���Ⱒ
     public float maxMotorTorque = 2500f; // �ִ� ��ũ(���ӷ�)
     public float maxBrakeTorque = 3500f; // �극��ũ ��ũ(���ӷ�)
     public float maxSpeed = 200f; // �극��ũ ��ũ(���ӷ�)
    public float emergencyBrakeForce = 80000;

     [Header("Car Current Speed")] public float carCurrentSpeed = 0f; // �극��ũ ��ũ(���ӷ�)


    private Rigidbody rb;

    public float SteerInput = 0f; //adŰ�� �޴� ȸ��
    public float motorInput; // ������ ������ũ�� �����ϱ� ���� ����
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
    private void Update() // Ű�Է� �޴� ��
    {
        this.SteerInput = Input.GetAxis("Horizontal");
        //this.forward = Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1); // wŰ�� �ޱ� ���� Clamp�� Ȱ���ؼ� 0 ���Ϸ� �ȳ���������
        //this.back = -1 * Mathf.Clamp(Input.GetAxis("Vertical"), -1, 0); // sŰ�� �ޱ� ���� Clamp�� Ȱ���ؼ� 0 �̻����� �ȿö󰡰���
        // -1�� ���ؼ� ����� ���� ����� �����ϰ� ��



        this.motorInput = Input.GetAxis("Vertical");
        /*if (Input.GetKey(KeyCode.W)) // ������ �϶�
        {
            StartCoroutine(ForwardCar());
        }
        else if (Input.GetKey(KeyCode.S)) // ������ �϶�
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



        /*//�̷����� ����°Ŷ� �޹����� ȸ����ų����
        #region �� ���� ȸ�� X��(���� �Ĺ�)
        this.BL_c.motorTorque = this.maxMotorTorque * motorInput;
        this.BR_c.motorTorque = this.maxMotorTorque * motorInput;
        #endregion
        //�չ����� �¿� ȸ�� ��ų����
        #region �� ���� ȸ�� Y��(�¿�)
        this.FL_c.steerAngle = this.SteerInput * this.maxSteerAngle;
        this.FR_c.steerAngle = this.SteerInput * this.maxSteerAngle;
        #endregion
        //�޹����� �극��ũ�ɸ� ������
        #region �극��ũ �ɱ�
        this.BL_c.brakeTorque = this.maxBrakeTorque * brakeInput;
        this.BR_c.brakeTorque = this.maxBrakeTorque * brakeInput;
        #endregion*/


        HandleMotor();


        
    }

    void HandleMotor()
    {
        //��� �극��ũ�� ���� ��, ���� ��� �������� �����ؾ���
        if (isEmergencyBraking)
        {
            //�޹��� �ӵ� 0����

            this.BL_c.motorTorque =0;
            this.BR_c.motorTorque =0;
                                  

            //�޹����� �극��ũ�ɸ� ������

            this.BL_c.brakeTorque = this.emergencyBrakeForce;
            this.BR_c.brakeTorque = this.emergencyBrakeForce;
            this.lightCtrl.BackLightCtrl(false);

        }
        else
        {
            //��� �극��ũ�� �ƴ϶��, �Ϲ����� ����
            //�̷����� ����°Ŷ� �޹����� ȸ����ų����
            #region �� ���� ȸ�� X��(���� �Ĺ�)
            this.BL_c.motorTorque = this.maxMotorTorque * motorInput;
            this.BR_c.motorTorque = this.maxMotorTorque * motorInput;
            #endregion
            //�չ����� �¿� ȸ�� ��ų����
            #region �� ���� ȸ�� Y��(�¿�)
            this.FL_c.steerAngle = this.SteerInput * this.maxSteerAngle;
            this.FR_c.steerAngle = this.SteerInput * this.maxSteerAngle;
            #endregion
            //�޹����� �ɾ��� �극��ũ ����

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
        #region �𵨸��� ȸ�� ��Ű��
        /* this.FL_m.localEulerAngles = new Vector3(FL_c.transform.localEulerAngles.x, this.SteerInput * maxSteerAngle, FL_c.transform.localEulerAngles.z);
         this.FR_m.localEulerAngles = new Vector3(FR_c.transform.localEulerAngles.x, this.SteerInput * maxSteerAngle, FR_c.transform.localEulerAngles.z);
         // �չ����� y�൵ ȸ�� ���Ѿ���

         FL_m.Rotate(this.BL_c.rpm * Time.fixedDeltaTime, 0, 0);
         FR_m.Rotate(this.BR_c.rpm * Time.fixedDeltaTime, 0, 0);
         BL_m.Rotate(this.BL_c.rpm * Time.fixedDeltaTime, 0, 0);
         BR_m.Rotate(this.BR_c.rpm * Time.fixedDeltaTime, 0, 0);
         //������ �����°� ��� ���� �� ��������
         //RPM = �д� ȸ����*/
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
