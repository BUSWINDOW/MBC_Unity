using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWheel : MonoBehaviour
{
    public WheelCollider targetWheel;
    public Vector3 wheelPos = Vector3.zero;
    public Quaternion wheelRot = Quaternion.identity;

    // Update is called once per frame
    void Update()
    {
        targetWheel.GetWorldPose(out wheelPos, out wheelRot);
        this.transform.position = wheelPos;
        this.transform.rotation = wheelRot;
    }
}
