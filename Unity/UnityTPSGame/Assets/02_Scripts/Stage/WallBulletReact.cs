using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WallBulletReact : MonoBehaviour
{
    //1. �浹 ���� , 2. ����ũ Ʀ 3. �Ҹ���
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
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal); // ���� ���Ͱ� �̷�� ȸ�� ���� ����
            //�ݻ簢
            //var spark = Instantiate(this.sparkEffect, contact.point, rot);
            var spark = PoolingManager.Instance.GetObj0710();
            spark.transform.position = contact.point; // �浹 ������ ��ġ ����
            spark.transform.rotation = rot; // ȸ�� ����

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
