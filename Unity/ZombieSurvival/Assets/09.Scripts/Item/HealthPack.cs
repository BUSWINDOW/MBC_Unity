using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour, IItem
{
    public int health = 50;
    public void Use(GameObject target)
    {
        Debug.Log("Health Pack used on " + target.name);
    }
}
