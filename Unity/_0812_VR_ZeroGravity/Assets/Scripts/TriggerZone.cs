using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.Oculus.Input;
using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
    public string targetTag = "Meteor";
    public Action<GameObject> onEnterAction;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(this.targetTag))
        {
            this.onEnterAction(other.gameObject);
        }
    }
}
