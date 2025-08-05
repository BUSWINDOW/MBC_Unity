using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxCtrl : MonoBehaviour
{
    RagDollCtrl rdCtrl;
    Rigidbody rb;
    private void Start()
    {
        this.rdCtrl = GetComponentInChildren<RagDollCtrl>();
        this.rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            this.rdCtrl.ActiveRagDoll();

        }
    }
}
