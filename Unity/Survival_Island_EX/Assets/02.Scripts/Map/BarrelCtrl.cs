using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    int hitCnt = 0;
    public Mesh[] bumped_Mesh;
    public Texture[] colors;
    public Rigidbody rb;
    private readonly static string bullet = "Bullet";
    void Start()
    {
        this.colors = Resources.LoadAll<Texture>("Barrel_Textures");
        this.GetComponent<MeshRenderer>().material.mainTexture = this.colors[Random.Range(0, this.colors.Length)];
        this.rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(bullet))
        {
            Debug.Log(this.hitCnt);
            if(++hitCnt >= 3)
            {
                Debug.Log("ลอมü");
                this.Explosion();
            }
        }
    }
    void Explosion()
    {
        this.rb.AddExplosionForce(50000, this.transform.position, 10, 50000);
        this.GetComponent<MeshFilter>().mesh = this.bumped_Mesh[Random.Range(0, this.bumped_Mesh.Length)];
    }
}
