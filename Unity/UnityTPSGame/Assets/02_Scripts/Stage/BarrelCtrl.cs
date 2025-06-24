using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    // 3�� ������ ���� ����
    // 1. ���� Ƚ��, 2. ���� �޽� 3. ���� ����Ʈ 4. ���� ���� 5. ���� ȿ��

    [SerializeField] GameObject explosionEffect;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip explosionClip;
    int hitCnt = 0;
    [SerializeField] Rigidbody rb;
    [SerializeField] Texture[] textures;
    [SerializeField] MeshRenderer _renderer;

    [SerializeField] MeshFilter filter;
    [SerializeField] Mesh[] bumpedMeshes;

    bool isExplode = false;


    private float radius = 20f;

    private static readonly string bullet = "Bullet";

    //public static Action explodAction;
    public static Action shakeAction;

    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
        this.source = GetComponent<AudioSource>();
        this.textures = Resources.LoadAll<Texture>("Textures");
        this._renderer = GetComponent<MeshRenderer>();
        this.filter = GetComponent<MeshFilter>();

        this._renderer.material.mainTexture = textures[UnityEngine.Random.Range(0, textures.Length)]; 
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(bullet))
        {
            if(++this.hitCnt >= 3)
            {
                this.Explosion();
            }
        }
    }
    private void Explosion()
    {
        if (this.isExplode)
        {
            return;
        }
        this.isExplode = true;
        var exp = Instantiate(explosionEffect, this.transform.position, Quaternion.identity);
        Destroy(exp, 1.4f);
        this.source.PlayOneShot(this.explosionClip);

        filter.sharedMesh = bumpedMeshes[UnityEngine.Random.Range(0, bumpedMeshes.Length)]; // ��׷��� �޽� �����ϴ� �ڵ�

        Collider[] cols = Physics.OverlapSphere(this.transform.position, radius, 1<< 13|1<<7);
        foreach (Collider col in cols)
        {
            var _rb = col.GetComponent<Rigidbody>();
            _rb.mass = 1;
            _rb.AddExplosionForce(800f, transform.position, this.radius,500f);
            //col.GetComponent<BarrelCtrl>().Explosion();
            col.gameObject.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
        }
        //var cam = Camera.main.GetComponent<Shake>();
        //cam.shakeRotate = true;
        //StartCoroutine(cam.ShakeCamera(1));
        //cam.ShakeTween(1);

        //explodAction();
        shakeAction();

    }
}
