using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    public Animator arm_Anim;
    void Start()
    {
        
    }
    void Update()
    {
        this.arm_Anim.SetBool("Zoom", Input.GetMouseButton(1));
    }
}
