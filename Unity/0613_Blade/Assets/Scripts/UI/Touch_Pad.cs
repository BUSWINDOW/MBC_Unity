using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(EventTrigger))]

public class Touch_Pad : MonoBehaviour
{
    [SerializeField]
    [Tooltip("터치패드")] private RectTransform touchPad; // 자기 자신의 RectTransform

    [SerializeField] private Vector3 startPos; // 시작 지점 위치
    [SerializeField] private float dragRadius; // 반지름, 조이패드 버튼이 조이패트 배경 밖으로 안나가게
    [SerializeField] private PlayerController playerController; // 패드의 움직임을 플레이어에게 전달

    private bool isPressed; // 버튼을 눌렀는지 안눌렀는지
    private int touchId = -1; // 마우스 포인터나 손가락이 원안에 있는지 체크

    void Start()
    {
        //초기화
        this.touchPad = this.GetComponent<RectTransform>();
        this.playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        this.isPressed = false;

        this.startPos = this.touchPad.position;

        this.dragRadius = 80f;
        
    }

    public void ButtonDown() 
    {

        this.isPressed = true;
    }
   

    public void ButtonUp()
    {
        this.isPressed = false;
        HandleInput(this.startPos);
    }

    void FixedUpdate() //고정 프레임, 정확한 물리량에 따른 것을 구현 //2d movePosition을 이걸로 구현
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                {
                    HandleTouchInput();
                    break;
                }
            case RuntimePlatform.IPhonePlayer:
                {
                    HandleTouchInput();
                    break;
                }
            case RuntimePlatform.WindowsEditor:
                {
                    HandleInput(Input.mousePosition);
                    break;
                }
        }
    }
    void HandleTouchInput() //모바일 기준 , 모바일 입력만 관리하는 메서드
    {
        int i = 0;
        if(Input.touchCount > 0)
        {
            foreach(var touch in Input.touches)
            {
                i++;
                Vector2 touchPos = new Vector2(touch.position.x, touch.position.y);
                if(touch.phase == TouchPhase.Began) //터치가 시작 되었을 때
                {
                    if(touch.position.x <= startPos.x + dragRadius) // 원 밖으로 벗어나지 않았다면
                    {
                        this.touchId = i;
                    }

                }
                if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    //터치 유형이 패드를 움직이고 있거나 원 안에서 멈춰있다면
                    if(touchId == i) // 그리고 터치가 시작 되었을 때의 터치라면
                    {
                        HandleInput(touchPos); // 실제로 움직이는 메서드 호출
                    }

                }
                if(touch.phase == TouchPhase.Ended)
                {
                    if (touchId == i)
                    this.touchId = -1;
                }
            }
        }
    }
    void HandleInput(Vector3 input) // WindowsEditor 기준 , 실제로 움직이는 메서드
    {
        if (this.isPressed)
        {
            //Debug.Log("눌리고있음");
            Vector3 differVector = (input - startPos);
            //전체 원의 거리의 크기
            if(differVector.sqrMagnitude > dragRadius * dragRadius)
                //sqrMagnitude : magnitude보다 연산이 빠름
            {
                //differVector.normalized; 정규화한 값을 어디다 넣는거(원본은 변경 없음)
                differVector.Normalize(); //원본 벡터를 정규화함(원본을 바꿈)
                touchPad.position = startPos + differVector * dragRadius;
            }
            else
            {
                this.touchPad.position = input;
            }
        }
        else
        {
            this.touchPad.position = startPos;
        }

        Vector3 differ = touchPad.position - startPos;
        //Debug.Log(differ);
        Vector3 normal = new Vector3(differ.x / dragRadius, differ.y / dragRadius);//드래그 최대 거리 대비 얼마나 땡겨놨는가
        //Debug.Log(normal);
        if(playerController != null)
        {
            //플레이어한테 위에서 구한 방향을 전달
            this.playerController.OnStickPos(normal);
        }
    }
}
