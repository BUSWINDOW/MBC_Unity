using System.Collections;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json.Linq;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using UnityEngine.InputSystem;

public class TPSPlayerInput : MonoBehaviour
{
    public string moveYAxisName = "Vertical";
    public string moveXAxisName = "Horizontal";
    public string rotateAxisName = "Mouse X";
    public string fireButtonName = "Fire1";
    public string reloadButtonName = "Reload";

    public bool isRun { get; private set; }

    public float MoveZ {  get; private set; }//감지된 움직임의 입력값
    public float MoveX { get; private set; }//감지된 회전의 입력값
    public float MouseX { get; private set; } //마우스 X축 회전값
    public bool Fire { get; private set; }
    public bool Reload { get;  set; }
    public bool Throw { get; set; }




    PlayerInput playerInput;
    InputActionMap mainActionMap;
    private InputAction moveAction;
    private InputAction attackAction;
    private InputAction lookAction;
    private InputAction sprintAction;
    private InputAction throwAction;

    void Start()
    {
        //컴포넌트 초기화
        this.playerInput = GetComponent<PlayerInput>();

        //Action Map 추출
        this.mainActionMap = this.playerInput.actions.FindActionMap("PlayerAction");

        this.moveAction = mainActionMap.FindAction("Move");
        this.attackAction = mainActionMap.FindAction("Fire");
        this.lookAction = mainActionMap.FindAction("Look");
        this.sprintAction = mainActionMap.FindAction("Sprint");
        this.throwAction = mainActionMap.FindAction("Throw");

        this.moveAction.performed += (ctx) =>
        {
            Vector2 dir = ctx.ReadValue<Vector2>();
            this.MoveX = dir.x;
            this.MoveZ = dir.y;
        };
        this.moveAction.canceled += (ctx) =>
        {
            this.MoveX = 0;
            this.MoveZ = 0;
        };
        this.attackAction.started += (ctx) =>
        {
            this.Fire = true;
        };
        this.attackAction.canceled += (ctx) =>
        {
            this.Fire = false;
        };
        this.lookAction.performed += (ctx) =>
        {
            this.MouseX = ctx.ReadValue<Vector2>().x;
        };
        this.lookAction.canceled += (ctx) =>
        {
            this.MouseX = 0;
        };
        this.sprintAction.performed += (ctx) =>
        {
            if (this.MoveZ > 0.1f)
                this.isRun = true;
            else
            {
                this.isRun = false;
            }
        };
        this.sprintAction.canceled += (ctx) =>
        {
            this.isRun = false;
        };
        this.throwAction.performed += (ctx) =>
        {
            this.Throw = true;
        };

    }
    /*private void Update()
    {
        //this.MouseX = Input.GetAxis(rotateAxisName);
        if (Input.GetKey(KeyCode.LeftShift) && this.MoveZ > 0.1f)
        {
            this.isRun = true;
        }
        else
        {
            this.isRun = false;
        }
    }*/
    /*void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
        {
            MoveZ = 0; MoveX = 0; Fire = false; Reload = false; isRun = false;
            return;
        }
        MoveZ = Input.GetAxis(moveYAxisName);
        MoveX = Input.GetAxis(moveXAxisName);
        Fire = Input.GetButton(fireButtonName);
        //Reload = Input.GetButtonDown(reloadButtonName);
        this.MouseX = Input.GetAxis(rotateAxisName);

        if (Input.GetKey(KeyCode.LeftShift)&& this.MoveZ > 0.1f)
        {
            this.isRun = true;
        }
        else
        {
            this.isRun = false;
        }
    }*/
    void OnMove(InputValue value)
    {
        Vector2 dir = value.Get<Vector2>();
        this.MoveX = dir.x;
        this.MoveZ = dir.y;
    }
    void OnFire(InputValue value)
    {
        this.Fire = value.Get<bool>();
    }
    void OnLook(InputValue value)
    {
        this.MouseX = value.Get<Vector2>().x;
    }

}
