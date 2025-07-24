using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivingEntity : MonoBehaviour
{
    public int hp;
    public int maxHp = 100;
    public Slider hpSlider;
    public bool isDead = false;

    protected virtual void Start()
    {
        this.maxHp = 100;
        this.hp = this.maxHp;
        this.hpSlider.maxValue = this.maxHp;
        this.hpSlider.wholeNumbers = true;
        this.hpSlider.value = this.hp;
    }

    public virtual void OnDamage(int damage)
    {
        this.hp -= damage;
        if (this.hp < 0)
        {
            this.hp = 0;
        }
        if(this.hp == 0)
        {
            this.isDead = true;
        }
        this.hpSlider.value = this.hp;
    }
}
