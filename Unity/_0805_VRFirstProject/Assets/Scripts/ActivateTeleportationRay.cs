using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActivateTeleportationRay : MonoBehaviour
{
    public GameObject leftTeleportationRay;
    public GameObject rightTeleportationRay;

    public InputActionProperty leftActivate;
    public InputActionProperty rightActivate;

    private void Update()
    {
        this.leftTeleportationRay.SetActive(leftActivate.action.ReadValue<float>() > 0.1f);
        this.rightTeleportationRay.SetActive(rightActivate.action.ReadValue<float>() > 0.1f);
    }
}
