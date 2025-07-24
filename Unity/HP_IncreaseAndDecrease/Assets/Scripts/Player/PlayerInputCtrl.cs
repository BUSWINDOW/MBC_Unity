using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputCtrl : MonoBehaviour
{
    private PlayerInput inputCtrl;
    private InputActionMap moveActionMap;
    private InputAction move;
    private InputAction granadeThrow;
    private PlayerHPCtrl playerCtrl;
    Vector2 dir = Vector2.zero;
    void Start()
    {
        this.inputCtrl = GetComponent<PlayerInput>();
        this.playerCtrl = GetComponent<PlayerHPCtrl>();
        this.moveActionMap = this.inputCtrl.actions.FindActionMap("PlayerCtrl");
        this.move = this.moveActionMap.FindAction("Move");
        this.granadeThrow = this.moveActionMap.FindAction("Throw");

        this.move.performed += (ctx) =>
        {
            this.dir = ctx.ReadValue<Vector2>();
            Debug.Log(this.dir);
            playerCtrl.MoveCtrl(this.dir);
        };
        this.move.canceled += (ctx) =>
        {
            playerCtrl.MoveCtrl(Vector2.zero);
        };
        this.granadeThrow.performed += (ctx) =>
        {
            this.playerCtrl.GranadeThrow();
        };
    }
}
