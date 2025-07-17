using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int hp;
    public int maxHp;

    public bool isDead = false;

    public List<Renderer> meshes;


    public CharacterController ctrl;
    public Animator anim;
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashRespawn = Animator.StringToHash("Respawn");

    private readonly string bullet = "BULLET";
    void Start()
    {
        this.GetComponentsInChildren<Renderer>(this.meshes);
        this.anim = GetComponent<Animator>();
        this.ctrl = GetComponent<CharacterController>();
        this.maxHp = 100;
        this.hp = this.maxHp;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (this.hp > 0 && collision.collider.CompareTag(bullet)) 
        {
            this.hp -= 1;
            if (this.hp <= 0)
            {
                StartCoroutine(PlayerDieRoutine());
            }
        }

    }

    IEnumerator PlayerDieRoutine()
    { 
        this.ctrl.enabled = false;
        this.anim.SetBool(this.hashRespawn, false);
        this.anim.SetTrigger(this.hashDie);

        yield return new WaitForSeconds(3f);
        this.SetVisible(false);
        yield return new WaitForSeconds(UnityEngine.Random.Range(1f,3f));

        this.hp = this.maxHp;

        Transform[] points = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();
        int idx = UnityEngine.Random.Range(1, points.Length);

        this.transform.position = points[idx].position;


        this.ctrl.enabled = true;
        this.anim.SetBool(this.hashRespawn, true);
        this.SetVisible(true);
    }
    void SetVisible(bool visible)
    {
        foreach(var mesh in this.meshes)
        {
            mesh.enabled = visible;
        }
    }
}
