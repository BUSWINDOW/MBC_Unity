using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharInput : MonoBehaviour
{
    PlayerCtrl ctrl;
    PlayerAnim animCtrl;
    private readonly int hashMove = Animator.StringToHash("Movement");
    private readonly int hashAttack = Animator.StringToHash("Attack");
    private void Start()
    {
        ctrl = GetComponent<PlayerCtrl>();
        animCtrl = GetComponent<PlayerAnim>();

        //CSharp Invoke����� ���� ����
        this.Start_For_CSharp_Invoke();
    }

    #region SendMessage ���
    void OnMove(InputValue value)
    {
        Vector2 dir = value.Get<Vector2>();
        ctrl.moveDir = new Vector3(dir.x, 0, dir.y);
        animCtrl.anim.SetFloat(hashMove, dir.magnitude);
    }
    void OnAttack(InputValue value)
    {
        animCtrl.anim.SetTrigger(hashAttack);
    }
    #endregion


    #region Invoke ���
    public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 dir = ctx.ReadValue<Vector2>();
        ctrl.moveDir = new Vector3(dir.x, 0, dir.y);
        animCtrl.anim.SetFloat(hashMove, dir.magnitude);
    }
    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("Attack");
            animCtrl.anim.SetTrigger(hashAttack);
        }
    }
    #endregion

    #region CSharp Invoke ���

    PlayerInput playerInput;
    InputActionMap mainActionMap;
    private InputAction moveAction;
    private InputAction attackAction;


    void Start_For_CSharp_Invoke()
    {
        //������Ʈ �ʱ�ȭ
        this.playerInput = GetComponent<PlayerInput>();

        //Action Map ����
        this.mainActionMap = this.playerInput.actions.FindActionMap("PlayerAction");

        this.moveAction = mainActionMap.FindAction("Move");
        this.attackAction = mainActionMap.FindAction("Attack");

        this.moveAction.performed += (ctx) =>
        {
            Vector2 dir = ctx.ReadValue<Vector2>();
            ctrl.moveDir = new Vector3(dir.x, 0, dir.y);
            animCtrl.anim.SetFloat(hashMove, dir.magnitude);
        };

        this.moveAction.canceled += (ctx) =>
        {
            ctrl.moveDir = Vector3.zero;
            animCtrl.anim.SetFloat(hashMove, 0);
        };

        this.attackAction.performed += (ctx) =>
        {
            animCtrl.anim.SetTrigger(hashAttack);
        };
    }

    #endregion
}
