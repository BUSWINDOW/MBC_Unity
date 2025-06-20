using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class SwatDamage : MonoBehaviour
{
    [SerializeField] ParticleSystem blood;
    private readonly string Bullet = "Bullet";
    public Action<int> hitAction;
    // Start is called before the first frame update
    void Start()
    {
        this.blood.Stop();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Bullet))
        {
            Debug.Log("Enemy Hit");
            collision.gameObject.SetActive(false);
            this.blood.Play();
            //this.hp -= collision.gameObject.GetComponent<BulletCtrl>().damage;
            this.hitAction(collision.gameObject.GetComponent<BulletCtrl>().damage);
            /*if (this.hp <= 0)
            {
                this.GetComponent<SwatAI>().state = EnemyAI.eState.Die;
            }*/
        }
    }
}
