using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
// AICar 완성 1.PathLine따라 자동차가 이동 하기  2. 장애물 피하기 3. 코너 돌때 속도 줄이기
public class AICar : MonoBehaviour
{
    public enum ePos//그냥 int로써 사용
    {
        Front_Left, Front_Right, Back_Left, Back_Right
    }

    [Header("바퀴")]
    [SerializeField] private List<WheelCollider> wheelColliders; //바퀴 콜라이더
    [SerializeField] private List<Transform> wheelModels; //바퀴 모델

    [Header("자동차 세부 설정")]
    public Vector3 centerOfMass = new Vector3(0, -0.5f, 0);
    [SerializeField] private float maxMotorTorque = 2500f; // 최대 모터 토크
    [SerializeField] private float maxSteeringAngle = 35f; // 최대 조향 각도
    [SerializeField] private float maxSpeed = 10f; // 최대 속도
    [SerializeField]private float currentSpeed = 0f; // 현재 속도
    [SerializeField]private float targetSpeed = 0f;
    private Rigidbody rb; // 자동차의 리지드바디

    [Header("길")]
    PathPoints path;
    [SerializeField] private int idx = 0;

    [Header("자동차 인공지능 설정")]
    [SerializeField] private float corneringSpeedReduction = 0.5f; // 코너링 시 속도 감소 비율

    [Header("센서 위치")]
    [SerializeField] private Vector3 sensorOffset = new Vector3(0, 0.5f, 1.5f); // 센서 위치 오프셋
    [SerializeField] private float sensorLength = 20f; // 센서 길이
    [SerializeField] private float sensorPos = 0.1f; // 옆쪽 센서 위치
    [SerializeField] private float sensorAngle = 30f; // 센서 각도
    private bool isAvoiding = false; // 장애물 회피 중인지 여부

    public bool timeCtrl = false;

    void Start()
    {
        if (timeCtrl)
        {
            Time.timeScale = 15;
            this.maxMotorTorque = 150;
            this.maxSpeed = 10;
        }
        else
        {
            Time.timeScale = 1f; // 게임 속도를 기본값으로 설정
            this.maxMotorTorque = 1200f; // 최대 모터 토크
            this.maxSpeed = 50f; // 최대 속도
        }

            this.rb = GetComponent<Rigidbody>();
        if (rb != null)
            this.rb.centerOfMass = this.centerOfMass;
        this.path = GameObject.Find("PathPoints").GetComponent<PathPoints>();
    }
    void FixedUpdate()
    {
        ApplySteer();
        CarSesor(); // 경로에 따른 회전 먼저 적용 후 장애물 회피 센서 작동
        Drive();
        CheckDistance();
    }
    void ApplySteer()
    {
        Vector3 relativeVector = this.transform.InverseTransformPoint(this.path.GetCurrentPoint(this.idx));
        // 월드 좌표를 차량의 로컬 좌표로 변환해서 상대적인 위치를 계산한다
        // 내 좌표 기준 저게 어느 위치에 있는지를 준다

        float newSteer = (relativeVector.x / relativeVector.magnitude) * this.maxSteeringAngle;
        //그 위치의 x좌표를 이용해서 조향각을 계산
        // 경로라인의 x좌표 / 경로라인의 길이 * 최대 조향각
        this.GetWheelCol(ePos.Front_Left).steerAngle = newSteer;
        this.GetWheelCol(ePos.Front_Right).steerAngle = newSteer;
        this.AdjustSpeedForConering(newSteer); // 코너링 시 속도 조절
    }
    
    void CarSesor() //장애물 회피를 위한 기능 
    {
        Vector3 baseSensorPos = this.transform.position +
            this.transform.forward * this.sensorOffset.z +
            this.transform.up * this.sensorOffset.y;
        this.isAvoiding = false;
        bool avoidWayCheck = false;
        float avoidMultiplier = 0f; // 장애물 회피를 위한 가중치
        #region --정면 중앙 센서--
        if (Physics.Raycast(baseSensorPos, this.transform.forward, sensorLength,1<<6))
        {
            Debug.DrawRay(baseSensorPos, this.transform.forward * sensorLength, Color.red);
            this.isAvoiding = true;

            avoidMultiplier = 0f;
        }
        #endregion
        #region --정면 우측 센서--
        if (Physics.Raycast(baseSensorPos + this.transform.right * this.sensorPos, this.transform.forward, sensorLength, 1<<6))
        {
            Debug.DrawRay(baseSensorPos + this.transform.right * this.sensorPos, this.transform.forward * sensorLength, Color.blue);
            this.isAvoiding = true;
            avoidWayCheck = true;
            avoidMultiplier -= 0.5f;
        }
        #endregion
        #region --정면 좌측 센서--
        if (Physics.Raycast(baseSensorPos - this.transform.right * this.sensorPos, this.transform.forward, sensorLength, 1 << 6))
        {
            Debug.DrawRay(baseSensorPos - this.transform.right * this.sensorPos, this.transform.forward * sensorLength, Color.green);
            this.isAvoiding = true;
            if (!avoidWayCheck)
            {
                avoidMultiplier += 0.5f;
            }

        }
        #endregion
        #region --정면 우측 대각 센서--
        if (Physics.Raycast(baseSensorPos + this.transform.right * this.sensorPos, Quaternion.AngleAxis(this.sensorAngle, this.transform.up) * this.transform.forward, sensorLength, 1<<6))
        {
            Debug.DrawRay(baseSensorPos + this.transform.right * this.sensorPos, Quaternion.AngleAxis(this.sensorAngle, this.transform.up) * this.transform.forward * sensorLength, Color.magenta);
            this.isAvoiding = true;

            avoidMultiplier -= 0.25f;
        }
        #endregion
        #region --정면 좌측 대각 센서--
        if (Physics.Raycast(baseSensorPos - this.transform.right * this.sensorPos, Quaternion.AngleAxis(-this.sensorAngle, this.transform.up) * this.transform.forward, sensorLength, 1 << 6))
        {
            Debug.DrawRay(baseSensorPos - this.transform.right * this.sensorPos, Quaternion.AngleAxis(-this.sensorAngle, this.transform.up) * this.transform.forward * sensorLength, Color.cyan);
            this.isAvoiding = true;

            avoidMultiplier += 0.25f;
        }
        #endregion
        if (isAvoiding)
        {
            this.GetWheelCol(ePos.Front_Left).steerAngle = this.maxSteeringAngle * avoidMultiplier;
            this.GetWheelCol(ePos.Front_Right).steerAngle = this.maxSteeringAngle * avoidMultiplier;
        }
    }
    void Drive() 
    {
        this.currentSpeed = 2f * Mathf.PI * this.GetWheelCol(ePos.Front_Left).radius * this.GetWheelCol(ePos.Front_Left).rpm * 60f / 1000;
        //2 *Pi * r = 원의 둘레 => 한바퀴 돌면 이동하는 거리
        //rpm => round per minute => 1분에 몇바퀴 도는가
        // 위에거랑 곱하면 1분에 이동하는 거리
        // * 60 -> 1시간에 이동하는 거리 (m단위)
        // / 1000 -> m단위였던걸 km로(1km = 1000m)
        // => 즉 1시간동안 이동거리(km/h)
        if (this.currentSpeed < this.targetSpeed)
        {
            this.GetWheelCol(ePos.Back_Left).motorTorque = this.maxMotorTorque;
            this.GetWheelCol(ePos.Back_Right).motorTorque = this.maxMotorTorque;
        }
        else
        {
            this.GetWheelCol(ePos.Back_Left).motorTorque = 0f;
            this.GetWheelCol(ePos.Back_Right).motorTorque = 0f;
        }
    }

    void AdjustSpeedForConering(float steer) //주행시 코너링 할 때  속도를 감속 하는 기능 
    {
        //각도 newSteer에 따라서 속도 조절
        float normalizedAngle = Mathf.Clamp01(Mathf.Abs(steer) / this.maxSteeringAngle); // 왼쪽으로 꺾든 오른쪽으로 꺾든 감속이 되도록 절대값 적용
        targetSpeed = Mathf.Lerp(maxSpeed, maxSpeed * corneringSpeedReduction, normalizedAngle);
    }
    void CheckDistance()
    {
        if (Vector3.Distance(this.transform.position, this.path.GetCurrentPoint(this.idx)) <= 2.5f)
        {
            this.path.GetNextPoint(ref this.idx);
        }
    }
    private void LateUpdate()
    {
        // 바퀴 모델을 WheelCollider의 위치와 회전에 맞추기
        for (int i = 0; i < wheelColliders.Count; i++)
        {
            Vector3 position;
            Quaternion rotation;
            wheelColliders[i].GetWorldPose(out position, out rotation);
            wheelModels[i].position = position;
            wheelModels[i].rotation = rotation;
        }
    }
    private WheelCollider GetWheelCol(ePos pos)
    {
        return this.wheelColliders[(int)pos];
    }
}


