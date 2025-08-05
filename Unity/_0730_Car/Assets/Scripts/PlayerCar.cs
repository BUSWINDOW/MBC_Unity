using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerCar : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] WheelCollider[] FwheelCols;
    [SerializeField] WheelCollider[] BwheelCols;

    float SteerInput;
    float motorInput;


    public Vector3 centerOfMass = new Vector3(0, 0, 0);

    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
        this.rb.centerOfMass = this.centerOfMass;
    }

    // Update is called once per frame
    void Update()
    {
        this.SteerInput = Input.GetAxis("Horizontal");


        this.motorInput = Input.GetAxis("Vertical");

        
    }
    private void FixedUpdate()
    {
        foreach (WheelCollider col in BwheelCols)
        {
            col.motorTorque = 2500 * this.motorInput;
        }
        foreach (WheelCollider col in FwheelCols)
        {
            /*var targetAngle = 35 * this.SteerInput;
            col.steerAngle = Mathf.Lerp(col.steerAngle, targetAngle, Time.deltaTime* 100);*/
            col.steerAngle = 35 * this.SteerInput;
        }
    }
}
