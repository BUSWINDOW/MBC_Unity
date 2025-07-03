using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AICar : MonoBehaviour
{
    public enum ePos//�׳� int�ν� ���
    {
        Front_Left, Front_Right, Back_Left, Back_Right
    }
    [SerializeField] List<WheelCollider> wheelColliders = new List<WheelCollider>();
    [SerializeField] List<Transform> wheelModels = new List<Transform>();

    Rigidbody rb;

    [Header("Car Setting")]
    public Vector3 centerOfMass = new Vector3(0, -0.5f, 0);
    [SerializeField] float maxSteerAngle = 35f;
    [SerializeField] float maxMotorTorque = 2500f;
    [SerializeField] float maxSpeed;
    [SerializeField] float emergencyBrakeForce;
    [Header("Car Status")] public float currentSpeed = 0;

    [Header("Path Line")]
    [SerializeField] List<Transform> NodeList = new List<Transform>();
    [SerializeField] int curNodeIdx = 0;

    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
        if (rb != null)
            this.rb.centerOfMass = this.centerOfMass;
        var pathArray = GameObject.Find("PathPoints").transform;
        if (pathArray != null)
        {
            pathArray.GetComponentsInChildren<Transform>(NodeList);
            //�̷����ϸ� NodeList�� ��� -> Add�� �Ȱ���
            this.NodeList.RemoveAt(0);
        }
    }
    private void FixedUpdate() // �̵��κ�(���� �ݶ��̴��� �̵���Ű�� �κ�)
    {
        ApplySteer();
        Drive();
        CheckWayPointDist();
    }
    void ApplySteer() // �չ����� path�� ���� ȸ���ϴ� �޼���
    {
        Vector3 relativeVector = this.transform.InverseTransformPoint(this.NodeList[curNodeIdx].position);
        // ���� ��ǥ�� ������ ���� ��ǥ�� ��ȯ�ؼ� ������� ��ġ�� ����Ѵ�
        // �� ��ǥ ���� ���� ��� ��ġ�� �ִ����� �ش�

        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        //�� ��ġ�� x��ǥ�� �̿��ؼ� ���Ⱒ�� ���
        // ��ζ����� x��ǥ / ��ζ����� ���� * �ִ� ���Ⱒ
        this.GetWheelCol(ePos.Front_Left).steerAngle = newSteer;
        this.GetWheelCol(ePos.Front_Right).steerAngle = newSteer;
    }
    void Drive() // path�� ���� �̵�
    {
        this.currentSpeed = 2f * Mathf.PI * this.GetWheelCol(ePos.Front_Left).radius * this.GetWheelCol(ePos.Front_Left).rpm * 60f / 1000;
        //2 *Pi * r = ���� �ѷ� => �ѹ��� ���� �̵��ϴ� �Ÿ�
        //rpm => round per minute => 1�п� ����� ���°�
        // �����Ŷ� ���ϸ� 1�п� �̵��ϴ� �Ÿ�
        // * 60 -> 1�ð��� �̵��ϴ� �Ÿ� (m����)
        // / 1000 -> m���������� km��(1km = 1000m)
        // => �� 1�ð����� �̵��Ÿ�(km/h)

        if (this.currentSpeed < maxSpeed)
        {
            GetWheelCol(ePos.Back_Left).motorTorque = this.maxMotorTorque;
            GetWheelCol(ePos.Back_Right).motorTorque = this.maxMotorTorque;
        }
        else
        {
            GetWheelCol(ePos.Back_Left).motorTorque = 0;
            GetWheelCol(ePos.Back_Right).motorTorque = 0;
        }
    }
    void CheckWayPointDist() // ��θ� üũ�ؼ� �ε����� �ٽ� 0����
    {
        if(Vector3.Distance(this.transform.position, this.NodeList[curNodeIdx].position) <= 2.5f)
        {
            if(this.curNodeIdx++ == this.NodeList.Count-1)
                { this.curNodeIdx = 0; }
        }
    }

    private WheelCollider GetWheelCol(ePos pos)
    {
        return this.wheelColliders[(int)pos];
    }
    private Transform GetWheelModel(ePos pos)
    {
        return this.wheelModels[(int)pos];
    }

    private void LateUpdate() // ���� �� ȸ�� ǥ���ϴ� �κ�
    {
        for(int pos = 0;pos < 4; pos++)
        {
            Vector3 wheel_Pos;
            Quaternion wheel_Rot;
            this.GetWheelCol((ePos)pos).GetWorldPose(out wheel_Pos, out wheel_Rot);
            this.GetWheelModel((ePos)pos).position = wheel_Pos;
            this.GetWheelModel((ePos)pos).rotation = wheel_Rot;
        }
    }
}
