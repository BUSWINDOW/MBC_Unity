using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public readonly string Horizontal = "Horizontal";
    public readonly string Vertical = "Vertical";
    public readonly string Fire1 = "Fire1";

    public float h;
    public float v;
    public bool isFire;
    private void Start()
    {
        
    }
    private void Update()
    {
        this.h = Input.GetAxis(Horizontal);
        this.v = Input.GetAxis(Vertical);
        this.isFire = Input.GetButtonDown(Fire1);
    }
}
