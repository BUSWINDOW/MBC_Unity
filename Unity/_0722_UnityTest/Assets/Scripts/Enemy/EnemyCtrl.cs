using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCtrl : LivingEntity
{
    protected override void Start()
    {
        base.Start();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 7)
        {
            collision.gameObject.GetComponent<LivingEntity>().OnDamage(10);
        }
    }
}
