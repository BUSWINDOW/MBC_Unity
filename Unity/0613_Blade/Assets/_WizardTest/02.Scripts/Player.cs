using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
// 메인카메라에서 ray를 쏘면 플레이어가 그곳으로 이동하는 스크립트
public class Player : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private new Transform transform;
    [SerializeField]
    [Tooltip("애니메이터 컴퍼넌트")]
    private Animator animator;
    [SerializeField]
    [Tooltip("내비게이션 메쉬 에이전트 컴퍼넌트")]
    private NavMeshAgent agent;
    [SerializeField]private float m_doubleClickSecond =0.25f;
    [SerializeField] private bool isOneClick = false; //원클릭 더블클릭 구분
    [SerializeField] private float m_Timer = 0f; //타이머  시간저장 
    private Vector3 target = Vector3.zero; //타겟 위치
    private string speed = "Speed"; //애니메이터의 스피드 파라미터 이름
    public  string Skill = "Skill"; //애니메이터의 스킬 파라미터 이름
    void Start()
    {
       
       
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
      
        CameraRay();
        ClickDoubleClick();
        MouseMovement();

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.A))
        {
            agent.isStopped = true; //정지
            agent.speed = 0f; //속도 0으로 설정
            agent.ResetPath(); //이동 경로 초기화
            agent.velocity = Vector3.zero; //속도 0으로 설정
            animator.SetTrigger(Skill);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            agent.isStopped = false; //시작
            animator.SetFloat(speed, agent.speed, 0.0002f, Time.deltaTime); //스피드 0으로 설정
        }

    }
    private void MouseMovement()
    {
        if (Input.GetMouseButtonDown(0)) //마우스 클릭
        {
            if (Physics.Raycast(ray, out hit, 100f, 1 << 8)) //광선이 맞으면
            {
                if (isOneClick)
                {
                    agent.speed = 3f; //원클릭시 정지
                    agent.isStopped = false;//추적
                }
                else
                {
                    agent.speed = 6f; //더블클릭시 이동속도
                    agent.isStopped = false; //추적
                }
                target = hit.point; //마우스포인트 찍은 위치를 타겟으로 넘김
                agent.destination = target; //타겟으로 이동
                animator.SetFloat(speed, agent.speed, 0.0002f, Time.deltaTime);
            }
        }
        else
        {
            if (agent.remainingDistance <= 0.5f) //목표지점에 도착하면
            {
                agent.isStopped = true; //정지
                animator.SetFloat(speed, 0f, 0.002f, Time.deltaTime); 
            }

        }
    }

    private void CameraRay()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //광선을 메인카메라에서 쏘고 마우스포지션  좌표를 알아낸다.
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
    }

    private void ClickDoubleClick()
    {
        if (isOneClick && (Time.time - m_Timer) > m_doubleClickSecond)
        {
            Debug.Log("원클릭!");
            isOneClick = false;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (!isOneClick)
            {
                m_Timer = Time.time;
                isOneClick = true;
            }
            else if (isOneClick && (Time.time - m_Timer) < m_doubleClickSecond)
            {
                Debug.Log("더블클릭!");
                isOneClick = false;
            }


        }
    }
}
