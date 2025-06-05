using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    private static string bullet = "Bullet";
    private AudioSource source;
    public ParticleSystem hitEffectPrefab;
    public AudioClip hitSound;
    void Start()
    {
        this.source = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(bullet))
        {
            var hitEff = Instantiate(hitEffectPrefab.gameObject,collision.transform.position, Quaternion.identity);
            Destroy(hitEff,3f);
            this.source.PlayOneShot(this.hitSound);
            Destroy(collision.gameObject.GetComponent<Rigidbody>());
            Destroy(collision.gameObject.GetComponent<SphereCollider>());
        }
    }
}
