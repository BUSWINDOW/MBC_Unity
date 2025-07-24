using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CineRPGPlayerInput : MonoBehaviour
{
    PlayerInput input;
    InputActionMap actionMap;
    InputAction moveInputAction;
    InputAction lookInputAction;
    InputAction attackInputAction;
    InputAction shieldattackInputAction;

    public Vector2 MoveDir {  get; set; }
    public float LookX { get; set; }
    public float LookY { get; set; }
    public bool Attack { get; set; }
    public bool Shield { get; set; }

    private void Start()
    {
        this.input = GetComponent<PlayerInput>();
        this.actionMap = this.input.actions.actionMaps[0];
        this.moveInputAction = this.actionMap.actions[0];
        this.lookInputAction = this.actionMap.actions[1];
        this.attackInputAction = this.actionMap.actions[2];
        this.shieldattackInputAction = this.actionMap.actions[3];


        this.moveInputAction.performed += (ctx) =>
        {
            this.MoveDir = ctx.ReadValue<Vector2>();
        };
        this.moveInputAction.canceled += (ctx) =>
        {
            this.MoveDir = Vector2.zero;
        };

        this.lookInputAction.performed += (ctx) =>
        {
            var dir = ctx.ReadValue<Vector2>();
            this.LookX = dir.x;
            this.LookY = dir.y;
        };
        this.lookInputAction.canceled += (ctx) =>
        {
            this.LookX = 0;
            this.LookY = 0;
        };

        this.attackInputAction.started += (ctx) =>
        {
            this.Attack = true;
        };
        this.shieldattackInputAction.started += (ctx) =>
        {
            this.Shield = true;
        };
        
    }
}
