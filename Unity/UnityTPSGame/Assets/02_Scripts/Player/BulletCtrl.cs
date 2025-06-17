using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletCtrl : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 2000f;
    public SphereCollider col;

    private void OnEnable()
    {
        this.rb = GetComponent<Rigidbody>();
        this.col = GetComponent<SphereCollider>();
        this.rb.AddForce(this.transform.forward * this.speed, ForceMode.Impulse);
    }
    private void OnDisable()
    {
        
    }
}
