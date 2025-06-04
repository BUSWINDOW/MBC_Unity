using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerAnimationCtrl : MonoBehaviour
{
    Animation playerAnimation;
    private static string running = "running";
    private static string idle = "idle";
    private static string fire = "Fire1";
    void Start()
    {
        this.playerAnimation = GetComponentInChildren<Animation>();
    }

    void Update()
    {
        PlayerMove();
        PlayerShoot();

    }

    private void PlayerShoot()
    {
        if (Input.GetButtonDown(fire))
        {
            this.playerAnimation.Play("fire");
        }
    }

    private void PlayerMove()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            this.playerAnimation.Blend(running, 4);
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            this.playerAnimation.Play(idle);
        }
    }
}
