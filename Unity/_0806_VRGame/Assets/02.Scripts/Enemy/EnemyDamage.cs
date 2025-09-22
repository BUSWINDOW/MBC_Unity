using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private readonly string bullet = "Bullet";
    public Action hitAction;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(bullet))
        {
            this.hitAction();
        }
    }
}
