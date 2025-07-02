using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private BoxCollider attackRange;
    [SerializeField] private MeshRenderer rangeGizmo;
    float swordDamage = 10;
    float shieldDamage = 5;
    void Start()
    {
        this.attackRange = GetComponentInChildren<BoxCollider>();
        this.rangeGizmo = this.attackRange.GetComponent<MeshRenderer>();
    }
    public void Gizmo_ON_OFF()
    {
        this.rangeGizmo.enabled = !this.rangeGizmo.enabled;
    }
}
