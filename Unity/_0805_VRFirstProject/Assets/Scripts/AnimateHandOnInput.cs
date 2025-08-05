using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHandOnInput : MonoBehaviour
{
    public InputActionProperty pinchAnimationAction;
    public InputActionProperty gripAnimationAction;
    public Animator handAnim;

    void Start()
    {
        this.handAnim = GetComponent<Animator>();
    }


    void Update()
    {
        var triggerValue = pinchAnimationAction.action.ReadValue<float>();
        var gripValue = gripAnimationAction.action.ReadValue<float>();
        this.handAnim.SetFloat("Trigger", triggerValue);
        this.handAnim.SetFloat("Grip", gripValue);
    }
}
