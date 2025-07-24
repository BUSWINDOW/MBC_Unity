using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    protected override void Start()
    {
        base.Start();
    }
    public override void OnDamage(int damage)
    {
        base.OnDamage(damage);
        this.gameObject.layer = 8;//피격 무적 layer
        StartCoroutine(UtilCode.WaitForSec(() =>
        {
            this.gameObject.layer = 7; // 일반 플레이어 layer
        }, 1f));
    }
}
