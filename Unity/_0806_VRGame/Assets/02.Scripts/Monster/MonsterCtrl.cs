using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCtrl : MonoBehaviour
{
    public int hp;
    public int maxHp;
    void Start()
    {
        this.maxHp = 100;
        this.hp = this.maxHp;
    }
    void Update()
    {
        
    }
}
