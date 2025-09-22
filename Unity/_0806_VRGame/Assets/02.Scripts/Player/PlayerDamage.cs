using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamage : MonoBehaviour
{
    private readonly string enemy_Attack = "Enemy_Attack";
    public Image test;
    public Action hitAction;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(this.enemy_Attack))
        {
            this.hitAction();
        }
    }
}
