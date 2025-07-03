using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AICar : MonoBehaviour
{
    public enum ePos//그냥 int로써 사용
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
            //이렇게하면 NodeList에 담김 -> Add와 똑같음
            this.NodeList.RemoveAt(0);
        }
    }
    private void FixedUpdate() // 이동부분(바퀴 콜라이더들 이동시키는 부분)
    {
        ApplySteer();
        Drive();
        CheckWayPointDist();
    }
    void ApplySteer() // 앞바퀴가 path를 따라서 회전하는 메서드
    {
        Vector3 relativeVector = this.transform.InverseTransformPoint(this.NodeList[curNodeIdx].position);
        // 월드 좌표를 차량의 로컬 좌표로 변환해서 상대적인 위치를 계산한다
        // 내 좌표 기준 저게 어느 위치에 있는지를 준다

        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        //그 위치의 x좌표를 이용해서 조향각을 계산
        // 경로라인의 x좌표 / 경로라인의 길이 * 최대 조향각
        this.GetWheelCol(ePos.Front_Left).steerAngle = newSteer;
        this.GetWheelCol(ePos.Front_Right).steerAngle = newSteer;
    }
    void Drive() // path를 따라서 이동
    {
        this.currentSpeed = 2f * Mathf.PI * this.GetWheelCol(ePos.Front_Left).radius * this.GetWheelCol(ePos.Front_Left).rpm * 60f / 1000;
        //2 *Pi * r = 원의 둘레 => 한바퀴 돌면 이동하는 거리
        //rpm => round per minute => 1분에 몇바퀴 도는가
        // 위에거랑 곱하면 1분에 이동하는 거리
        // * 60 -> 1시간에 이동하는 거리 (m단위)
        // / 1000 -> m단위였던걸 km로(1km = 1000m)
        // => 즉 1시간동안 이동거리(km/h)

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
    void CheckWayPointDist() // 경로를 체크해서 인덱스를 다시 0으로
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

    private void LateUpdate() // 바퀴 모델 회전 표현하는 부분
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
