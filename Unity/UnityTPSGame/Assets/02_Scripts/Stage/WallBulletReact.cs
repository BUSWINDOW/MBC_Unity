using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBulletReact : MonoBehaviour
{
    //1. 충돌 감지 , 2. 스파크 튐 3. 소리남
    public GameObject sparkEffect;
    public AudioSource source;
    public AudioClip hitClip;
    private readonly static string bullet = "Bullet";
    void Start()
    {
        this.source = GetComponent<AudioSource>();
        this.sparkEffect = Resources.Load("FlareMobile") as GameObject;
        this.hitClip = Resources.Load("Sounds/bullet_hit_metal_enemy_4") as AudioClip;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(bullet))
        {
            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal); // 법선 벡터가 이루는 회전 각도 추출
            //반사각
            var spark = Instantiate(this.sparkEffect, contact.point, rot);

            //var spark = Instantiate(this.sparkEffect, collision.transform.position,Quaternion.identity);
            this.source.PlayOneShot(this.hitClip);
            Destroy(spark, 5);
        }
    }
}
