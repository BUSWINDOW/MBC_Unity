using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class BarrelCtrl : MonoBehaviour
{
    int hitCnt = 0;
    public Mesh[] bumped_Mesh;
    public Texture[] colors;
    public Rigidbody rb;

    public static Action OnExplodAction;

    private readonly static string bullet = "Bullet";
    void Start()
    {
        this.colors = Resources.LoadAll<Texture>("Barrel_Textures");
        this.GetComponent<MeshRenderer>().material.mainTexture = this.colors[UnityEngine.Random.Range(0, this.colors.Length)];
        this.rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(bullet))
        {
            Debug.Log(this.hitCnt);
            if(++hitCnt >= 3)
            {
                Debug.Log("터짐");
                this.Explosion();
            }
        }
    }
    void Explosion()
    {
        this.rb.AddExplosionForce(50000, this.transform.position, 10, 50000);
        this.GetComponent<MeshFilter>().mesh = this.bumped_Mesh[UnityEngine.Random.Range(0, this.bumped_Mesh.Length)];
        
        var cam = Camera.main.transform;
        cam.DOShakePosition(1);
        //cam.DOShakeRotation(1, new Vector3(0,0,10)); //회전에다 흔들림 넣으니 이상해짐

        OnExplodAction();
    }
}
