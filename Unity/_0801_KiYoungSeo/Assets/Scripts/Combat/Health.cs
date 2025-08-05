using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        public int hp;
        public int maxHp;
        public bool isDie;
        protected Animator anim;

        private readonly int hashDie = Animator.StringToHash("Die");
        public virtual void Die()
        {
            this.anim.SetTrigger(hashDie);
        }
        
    }
}

