using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] ParticleSystem blood;
    private readonly string e_Bullet = "E_Bullet";

    public Action hitAction;
    
    // Start is called before the first frame update
    void Start()
    {
        this.blood.Stop();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(e_Bullet))
        {
            Debug.Log("Hit");
            collision.gameObject.SetActive(false);
            this.blood.Play();
            this.hitAction();
        }
    }

}
