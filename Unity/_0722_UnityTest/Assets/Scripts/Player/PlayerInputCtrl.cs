using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputCtrl : MonoBehaviour
{
    PlayerHealth player;

    PlayerInput input;
    InputActionMap playerActionMap;
    InputAction moveInputAction;
    InputAction jumpInputAction;
    InputAction lookInputAction;
    InputAction lookYInputAction;
    InputAction fireInputAction;

    public Vector2 moveDir;
    public bool Jump { get; set; }
    public bool Fire { get; set; }
    public float Look { get; set; }
    public float LookY { get; set; }


    void Start()
    {
        this.input = GetComponent<PlayerInput>();
        this.player = GetComponent<PlayerHealth>();
        this.playerActionMap = this.input.actions.actionMaps[0];
        this.moveInputAction = this.playerActionMap.actions[0];
        this.jumpInputAction = this.playerActionMap.actions[1];
        this.lookInputAction = this.playerActionMap.actions[2];
        this.fireInputAction = this.playerActionMap.actions[3];
        this.lookYInputAction = this.playerActionMap.actions[4];
        
        //this.lookInputAction = this.playerActionMap.FindAction("Look");


        this.moveInputAction.performed += (ctx) =>
        {
            if (!this.player.isDead)
                this.moveDir = ctx.ReadValue<Vector2>();
        };
        this.moveInputAction.canceled += (ctx) =>
        {
            this.moveDir = Vector2.zero;
        };

        this.jumpInputAction.started += (ctx) => 
        {
            if (!this.player.isDead)
                this.Jump = true;
        };

        this.lookInputAction.performed += (ctx) => 
        {
            if (!this.player.isDead)
                this.Look = ctx.ReadValue<float>();
        };
        this.lookInputAction.canceled += (ctx) =>
        {
            this.Look = 0;
        };
        this.fireInputAction.started += (ctx) =>
        {
            if (!this.Fire && !this.player.isDead)
                this.Fire = true;
        };
        this.fireInputAction.canceled += (ctx) =>
        {
            this.Fire = false;
        };
        this.lookYInputAction.performed += (ctx) =>
        {
            if (!this.player.isDead)
            {
                this.LookY = ctx.ReadValue<float>();
            }    
        };
        this.lookYInputAction.canceled += (ctx) =>
        {
            this.LookY = 0;
        };
    }

}
