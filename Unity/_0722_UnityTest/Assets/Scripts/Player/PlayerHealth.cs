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
        this.gameObject.layer = 8;//�ǰ� ���� layer
        StartCoroutine(UtilCode.WaitForSec(() =>
        {
            this.gameObject.layer = 7; // �Ϲ� �÷��̾� layer
        }, 1f));
    }
}
