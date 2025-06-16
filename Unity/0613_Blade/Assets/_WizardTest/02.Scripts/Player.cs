using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
// ����ī�޶󿡼� ray�� ��� �÷��̾ �װ����� �̵��ϴ� ��ũ��Ʈ
public class Player : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private new Transform transform;
    [SerializeField]
    [Tooltip("�ִϸ����� ���۳�Ʈ")]
    private Animator animator;
    [SerializeField]
    [Tooltip("������̼� �޽� ������Ʈ ���۳�Ʈ")]
    private NavMeshAgent agent;
    [SerializeField]private float m_doubleClickSecond =0.25f;
    [SerializeField] private bool isOneClick = false; //��Ŭ�� ����Ŭ�� ����
    [SerializeField] private float m_Timer = 0f; //Ÿ�̸�  �ð����� 
    private Vector3 target = Vector3.zero; //Ÿ�� ��ġ
    private string speed = "Speed"; //�ִϸ������� ���ǵ� �Ķ���� �̸�
    public  string Skill = "Skill"; //�ִϸ������� ��ų �Ķ���� �̸�
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
            agent.isStopped = true; //����
            agent.speed = 0f; //�ӵ� 0���� ����
            agent.ResetPath(); //�̵� ��� �ʱ�ȭ
            agent.velocity = Vector3.zero; //�ӵ� 0���� ����
            animator.SetTrigger(Skill);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            agent.isStopped = false; //����
            animator.SetFloat(speed, agent.speed, 0.0002f, Time.deltaTime); //���ǵ� 0���� ����
        }

    }
    private void MouseMovement()
    {
        if (Input.GetMouseButtonDown(0)) //���콺 Ŭ��
        {
            if (Physics.Raycast(ray, out hit, 100f, 1 << 8)) //������ ������
            {
                if (isOneClick)
                {
                    agent.speed = 3f; //��Ŭ���� ����
                    agent.isStopped = false;//����
                }
                else
                {
                    agent.speed = 6f; //����Ŭ���� �̵��ӵ�
                    agent.isStopped = false; //����
                }
                target = hit.point; //���콺����Ʈ ���� ��ġ�� Ÿ������ �ѱ�
                agent.destination = target; //Ÿ������ �̵�
                animator.SetFloat(speed, agent.speed, 0.0002f, Time.deltaTime);
            }
        }
        else
        {
            if (agent.remainingDistance <= 0.5f) //��ǥ������ �����ϸ�
            {
                agent.isStopped = true; //����
                animator.SetFloat(speed, 0f, 0.002f, Time.deltaTime); 
            }

        }
    }

    private void CameraRay()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //������ ����ī�޶󿡼� ��� ���콺������  ��ǥ�� �˾Ƴ���.
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
    }

    private void ClickDoubleClick()
    {
        if (isOneClick && (Time.time - m_Timer) > m_doubleClickSecond)
        {
            Debug.Log("��Ŭ��!");
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
                Debug.Log("����Ŭ��!");
                isOneClick = false;
            }


        }
    }
}
