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
            shooter.gun.ammo += ammo; // 총기의 탄약을 증가시킴
        }
        Destroy(gameObject); // 아이템 사용 후 제거

    }
}
