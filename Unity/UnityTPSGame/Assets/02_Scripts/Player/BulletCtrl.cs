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
    public int damage;
    private void Start()
    {
        GameManager.ItemChangeAction += this.UpdateSetUp;
    }

    private void OnEnable()
    {
        this.rb = GetComponent<Rigidbody>();
        this.col = GetComponent<SphereCollider>();
        this.damage = (int)GameManager.Instance.gameData.damage;

        

        this.rb.AddForce(this.transform.forward * this.speed, ForceMode.Impulse);
        StartCoroutine(this.WaitSomeSecond(() => 
        {
            this.gameObject.SetActive(false);
        } , 5));
    }
    void UpdateSetUp()
    {
        this.damage = (int)GameManager.Instance.gameData.damage;
    }
    private void OnCollisionEnter(Collision collision)
    {
        this.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        this.GetComponent<TrailRenderer>().Clear();
        this.rb.Sleep();

        //GameManager.ItemChangeAction -= this.UpdateSetUp;

        /*this.rb.velocity = Vector3.zero;
        this.rb.angularVelocity = Vector3.zero;*/
    }
    IEnumerator WaitSomeSecond(Action action , float second)
    {
        yield return new WaitForSeconds(second);
        action();
    }
}
