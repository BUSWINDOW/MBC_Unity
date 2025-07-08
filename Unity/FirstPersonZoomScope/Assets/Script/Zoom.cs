using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Zoom : MonoBehaviour
{
    public Animator arm_Anim;
    public Animator gun_Anim;

    public FirstPersonController firstPersonController;

    public float reloadTime = 1;
    public float t = 0;

    void Start()
    {
        this.firstPersonController = GetComponent<FirstPersonController>();
    }
    void Update()
    {
        this.t += Time.deltaTime;
        this.arm_Anim.SetBool("Zoom", Input.GetMouseButton(1));
        this.firstPersonController.m_MouseLook.XSensitivity = Input.GetMouseButton(1) ? 0.5f : 2f;
        this.firstPersonController.m_MouseLook.YSensitivity = Input.GetMouseButton(1) ? 0.5f : 2f;
        if (Input.GetMouseButtonDown(0) && t>= this.reloadTime)
        {
            Shoot();
            this.t = 0;
        }


    }

    private void Shoot()
    {
        this.gun_Anim.SetTrigger("Shoot");
    }
}
