using System;
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
        StartCoroutine(this.WaitSomeSecond(() => 
        {
            this.gameObject.SetActive(false);
        } , 5));
    }
    private void OnCollisionEnter(Collision collision)
    {
        this.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        this.GetComponent<TrailRenderer>().Clear();
        this.rb.Sleep();
        /*this.rb.velocity = Vector3.zero;
        this.rb.angularVelocity = Vector3.zero;*/
    }
    IEnumerator WaitSomeSecond(Action action , float second)
    {
        yield return new WaitForSeconds(second);
        action();
    }
}
