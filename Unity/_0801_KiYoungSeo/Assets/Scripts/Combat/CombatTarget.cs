using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour
    {
        Health health;
        public Action dieAction;
        Image hpBar;
        private void Awake()
        {
            this.health = GetComponent<Health>();
            this.health.maxHp = 100;
            
            this.hpBar = GetComponentsInChildren<Image>().ToList().Find(a => a.type == Image.Type.Filled);
        }
        private void OnEnable()
        {
            this.health.hp = this.health.maxHp;
            this.hpBar.fillAmount = (float)this.health.hp / (float)this.health.maxHp;
        }
        public void GetHit(int damage)
        {
            //체력 깎이고,ui에 반영시키고, 0되면 dieAction호출
            this.health.hp -= damage;
            this.hpBar.fillAmount = (float)this.health.hp / (float)this.health.maxHp;
            if (this.health.hp <= 0)
            {
                this.health.hp = 0;
                this.health.Die();
                dieAction();
                //죽는 애니메이션 재생 후 비활성화
                StartCoroutine(UtilScripts.WaitForSec(() =>
                {
                    this.gameObject.SetActive(false);
                }, 2f));
            }
        }
    }
}

