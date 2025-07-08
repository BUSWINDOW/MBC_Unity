using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerAnimationCtrl : MonoBehaviour
{
    Animation playerAnimation;
    private static string running = "running";
    private static string idle = "idle";
    private static string fire = "fire";
    private static string reloading = "pump1";
    private static string zoom = "zoom";
    void Start()
    {
        this.playerAnimation = GetComponentInChildren<Animation>();
    }


    public void PlayerShoot()
    {
        this.playerAnimation.Play(fire);
    }
    public void PlayerReloading()
    {
        this.playerAnimation.Play(reloading);
    }
    public void PlayerRunning()
    {
        this.playerAnimation.Blend(running, 4);
    }
    public void PlayerStop()
    {
        this.playerAnimation.Play(idle);
    }
    public void PlayerZoom()
    {
        this.playerAnimation.Play(zoom);
    }
    
}
