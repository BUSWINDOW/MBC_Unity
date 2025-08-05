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
    [SerializeField] float maxBrakeTorque;
    [Header("Car Status")] 
    public float currentSpeed = 0;
    public float targetSpeed = 0;

    [Header("Path Line")]
    [SerializeField] List<Transform> NodeList = new List<Transform>();
    [SerializeField] int curNodeIdx = 0;

    [Header("AI Driving Setting")]
    [SerializeField] private float steeringSharpness = 10f; //코너링 감속 민감도
    [SerializeField] private float lookAheadDistance = 5f; // 전방 주시 거리
    [SerializeField] private float corneringSpeedFactor = 0f; // 코너링시 속도 감소 비율


    [Header("Sensors")]
    [SerializeField] private float sensorLength = 20f; // 센서 길이
    [SerializeField] private Vector3 frontSensorPosition = new Vector3(0, 0.5f, 1.5f); //전방 센서 위치
    public float frontSideSensorPosition = 2f; // 전방 측면 센서 위치
    public float frontSensorAngle = 30f; //전방 센서 각도
    private bool avoiding = false; // 장애물 회피중인지 여부


    void Start()
    {
        Time.timeScale = 15;
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
        AdjustSpeedForCornering();
        
        ApplySteer();
        CarSensor();
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
        //각도 newSteer에 따라서 속도 조절
        float normalizedAngle = Mathf.Clamp01(newSteer / 180f);
        targetSpeed = Mathf.Lerp(maxSpeed, maxSpeed * corneringSpeedFactor, normalizedAngle);
    }

    
    void CarSensor()
    {
        RaycastHit hit;
        Vector3 baseSensorPos = this.transform.position +
            this.transform.forward * this.frontSensorPosition.z +
            this.transform.up * this.frontSensorPosition.y;
        this.avoiding = false;
        bool avoidWayCheck = false;
        float avoidMultiplier = 0f; // 장애물 회피를 위한 가중치
        #region --정면 중앙 센서--
        if(Physics.Raycast(baseSensorPos , this.transform.forward , out hit, sensorLength))
        {
            this.avoiding = true;

            avoidMultiplier = 0f;
        }
        #endregion
        #region --정면 우측 센서--
        if (Physics.Raycast(baseSensorPos + this.transform.right * this.frontSideSensorPosition, this.transform.forward, out hit, sensorLength))
        {
            this.avoiding = true;
            avoidWayCheck = true;
            avoidMultiplier -= 2f;
        }
        #endregion
        #region --정면 좌측 센서--
        if (Physics.Raycast(baseSensorPos - this.transform.right * this.frontSideSensorPosition, this.transform.forward, out hit, sensorLength))
        {
            this.avoiding = true;
            if (!avoidWayCheck)
            {
                avoidMultiplier += 2f;
            }
            
        }
        #endregion
        #region --정면 우측 대각 센서--
        if (Physics.Raycast(baseSensorPos - this.transform.right * this.frontSideSensorPosition, Quaternion.AngleAxis(this.frontSensorAngle,this.transform.up) * this.transform.forward, out hit, sensorLength))
        {
            this.avoiding = true;

            avoidMultiplier -= 1f;
        }
        #endregion
        #region --정면 좌측 대각 센서--
        if (Physics.Raycast(baseSensorPos - this.transform.right * this.frontSideSensorPosition, Quaternion.AngleAxis(-this.frontSensorAngle, this.transform.up) * this.transform.forward, out hit, sensorLength))
        {
            this.avoiding = true;

            avoidMultiplier += 1f;
        }
        #endregion

        if (avoiding)
        {
            this.GetWheelCol(ePos.Front_Left).steerAngle = this.maxSteerAngle * avoidMultiplier;
            this.GetWheelCol(ePos.Front_Right).steerAngle = this.maxSteerAngle * avoidMultiplier;
        }
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

        if (this.currentSpeed < targetSpeed)
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
    void AdjustSpeedForCornering()
    {

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
