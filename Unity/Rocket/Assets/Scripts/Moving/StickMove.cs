using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickMove : MonoBehaviour
{
    [SerializeField]
    [Tooltip("��ġ�е�")] private RectTransform touchPad; // �ڱ� �ڽ��� RectTransform

    [SerializeField] private Vector3 startPos; // ���� ���� ��ġ
    [SerializeField] private float dragRadius; // ������, �����е� ��ư�� ������Ʈ ��� ������ �ȳ�����
    [SerializeField] private Rocket player;

    private bool isPressed; // ��ư�� �������� �ȴ�������
    private int touchId = -1; // ���콺 �����ͳ� �հ����� ���ȿ� �ִ��� üũ

    void Start()
    {
        //�ʱ�ȭ
        this.touchPad = this.GetComponent<RectTransform>();
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
        HandleInput(this.startPos); //���� ��ġ�� �ǵ���
    }

    void FixedUpdate() //���� ������, ��Ȯ�� �������� ���� ���� ���� //2d movePosition�� �̰ɷ� ����
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
    void HandleTouchInput() //����� ���� , ����� �Է¸� �����ϴ� �޼���
    {
        int i = 0;
        if (Input.touchCount > 0)
        {
            foreach (var touch in Input.touches)
            {
                i++;
                Vector2 touchPos = new Vector2(touch.position.x, touch.position.y);
                if (touch.phase == TouchPhase.Began) //��ġ�� ���� �Ǿ��� ��
                {
                    if (touch.position.x <= startPos.x + dragRadius) // �� ������ ����� �ʾҴٸ�
                    {
                        this.touchId = i;
                    }

                }
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    //��ġ ������ �е带 �����̰� �ְų� �� �ȿ��� �����ִٸ�
                    if (touchId == i) // �׸��� ��ġ�� ���� �Ǿ��� ���� ��ġ���
                    {
                        HandleInput(touchPos); // ������ �����̴� �޼��� ȣ��
                    }

                }
                if (touch.phase == TouchPhase.Ended)
                {
                    if (touchId == i)
                        this.touchId = -1;
                }
            }
        }
    }
    void HandleInput(Vector3 input) // WindowsEditor ���� , ������ �����̴� �޼���
    {
        if (this.isPressed)
        {
            //Debug.Log("����������");
            Vector3 differVector = (input - startPos);
            //��ü ���� �Ÿ��� ũ��
            if (differVector.sqrMagnitude > dragRadius * dragRadius)
            //sqrMagnitude : magnitude���� ������ ����
            {
                //differVector.normalized; ����ȭ�� ���� ���� �ִ°�(������ ���� ����)
                differVector.Normalize(); //���� ���͸� ����ȭ��(������ �ٲ�)
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
        Vector3 normal = new Vector3(differ.x / dragRadius, differ.y / dragRadius);//�巡�� �ִ� �Ÿ� ��� �󸶳� ���ܳ��°�
        //Debug.Log(normal);
        if (player != null)
        {
            //�÷��̾����� ������ ���� ������ ����
            this.player.GetInputMove(normal);
        }
    }
}
