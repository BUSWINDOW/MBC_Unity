using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MariaInput : MonoBehaviour
{
    private const string horizontal = "Horizontal";
    private const string vertical = "Vertical";
    private const string mouseY = "Mouse Y";
    private const string mouseX = "Mouse X";

    float mouseXInput;
    float mouseYInput;
    float horizontalRawInput;
    float verticalRawInput;
    float horizontalInput;
    float verticalInput;
    float mouseScroll;
    bool fire1;
    bool fire2;

    void Update()
    {
        this.mouseXInput = Input.GetAxisRaw("Mouse X");
        this.mouseYInput = Input.GetAxisRaw("Mouse Y");
        this.horizontalRawInput = Input.GetAxisRaw("Horizontal");
        this.verticalRawInput = Input.GetAxisRaw("Vertical");
        this.horizontalInput = Input.GetAxis("Horizontal");
        this.verticalInput = Input.GetAxis("Vertical");
        this.fire1 = Input.GetButtonDown("Fire1");
        this.fire2 = Input.GetButtonDown("Fire2");
        this.mouseScroll = Input.GetAxis("Mouse ScrollWheel");
    }
    public float GetHorizontal()
    {
        return horizontalInput;
    }
    public float GetVertical()
    {
        return verticalInput;
    }
    public float GetHorizontalRaw()
    {
        return horizontalRawInput;
    }
    public float GetVerticalRaw()
    {
        return verticalRawInput;
    }
    public float GetMouseY()
    {
        return this.mouseYInput;
    }
    public float GetMouseX()
    {
        return this.mouseXInput;
    }
    public float GetMouseScroll()
    {
        return this.mouseScroll;
    }
    public bool GetFire1()
    {
        return this.fire1;
    }
    public bool GetFire2() 
    {
        return this.fire2;
    }
}
