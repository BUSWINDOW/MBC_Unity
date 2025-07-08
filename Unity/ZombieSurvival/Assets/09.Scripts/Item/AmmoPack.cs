using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPack : MonoBehaviour , IItem
{
    public int ammo = 10;
    // Implement the Use method from IItem interface
    public void Use(GameObject target)
    {
        var shooter = target.GetComponent<WomanShooter>();
        if (shooter != null && shooter.gun != null)
        {
            shooter.gun.ammo += ammo; // �ѱ��� ź���� ������Ŵ
        }
        Destroy(gameObject); // ������ ��� �� ����

    }
}
