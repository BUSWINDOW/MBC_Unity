using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Movement;
using System;
using RPG.Combat;
using RPG.Core;
using UnityEngine.InputSystem;
namespace RPG.Control
{
    public class PlayerController : Health
    {

        int groundLayerMask = 1 << 6;
        int enemyLayerMask = 1 << 7;
        Ray ray;
        RaycastHit hit;
        Mover mover;
        Fighter fighter;
        PlayerInput _input;

        InputAction leftClick;
        InputAction rightClick;

        void Awake()
        {
            this.mover = GetComponent<Mover>();
            this.mover.Init();
            this.fighter = GetComponent<Fighter>();
            this.fighter.Init();
            this.anim = GetComponent<Animator>();
            this._input = GetComponent<PlayerInput>();
        }
        private void Start()
        {
            this.leftClick = this._input.actions.actionMaps[0].actions[0];
            this.rightClick = this._input.actions.actionMaps[0].actions[1];
        }
        private void OnEnable()
        {
            this.isDie = false;
        }
        void Update()
        {
            if (isDie)
            {
                return;
            }
            if (this.rightClick.triggered) //우클릭 하면
            {

                this.ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(this.ray, out hit, Mathf.Infinity, this.groundLayerMask))
                {
                    this.fighter.Cancel();
                    this.mover.MoveSet(hit.point, 3.5f);
                }

            }

            if (this.leftClick.triggered) //좌클릭 하면
            {

                this.ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(this.ray, out hit, Mathf.Infinity, this.enemyLayerMask))
                {
                    this.fighter.Cancel();
                    this.fighter.AttackSet(hit.transform.gameObject);
                }

            }

        }
        public override void Die()
        {
            base.Die();
            this.fighter.Cancel();
            this.isDie = true;
        }
    }
}
