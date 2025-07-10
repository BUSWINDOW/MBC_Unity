using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

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
            //var spark = Instantiate(this.sparkEffect, contact.point, rot);
            var spark = PoolingManager.Instance.GetObj0710();
            spark.transform.position = contact.point; // 충돌 지점에 위치 설정
            spark.transform.rotation = rot; // 회전 설정

            //var spark = Instantiate(this.sparkEffect, collision.transform.position,Quaternion.identity);
            //this.source.PlayOneShot(this.hitClip);
            SoundManager.Instance.playSFX(contact.point, this.hitClip, false);
            //Camera.main.GetComponent<Shake>();
            //Destroy(spark, 5);
            StartCoroutine(WaitForDestroy(() =>
            {
                spark.SetActive(false);
            }, 5f));
        }
    }
    IEnumerator WaitForDestroy(Action act , float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        act();
    }
}
