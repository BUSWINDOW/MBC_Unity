using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] ParticleSystem blood;
    private readonly string Bullet = "Bullet";
    int hp;
    // Start is called before the first frame update
    void Start()
    {
        this.hp = 40;
        this.blood.Stop();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Bullet))
        {
            Debug.Log("Enemy Hit");
            collision.gameObject.SetActive(false);
            this.blood.Play();
            this.hp -= collision.gameObject.GetComponent<BulletCtrl>().damage;
            if(this.hp <= 0)
            {
                this.GetComponent<EnemyAI>().state = EnemyAI.eState.Die;
            }
        }
    }
}
