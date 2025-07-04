using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPack : MonoBehaviour , IItem
{
    public int ammo = 10;
    // Implement the Use method from IItem interface
    public void Use(GameObject target)
    {
        //Åº¾à Ãß°¡
        Debug.Log("Ammo Pack used on " + target.name);
    }
}
