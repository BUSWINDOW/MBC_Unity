using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] ParticleSystem blood;
    private readonly string e_Bullet = "E_Bullet";

    public Action hitAction;

    [SerializeField] private Image bloodScreen;
    
    
    // Start is called before the first frame update
    void Start()
    {
        this.blood.Stop();
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(e_Bullet))
        {
            Debug.Log("Hit");
            collision.gameObject.SetActive(false);
            this.blood.Play();
            this.hitAction();

            this.bloodScreen.DOFade(1, 0.2f).OnComplete(() =>
            {
                this.bloodScreen.DOFade(0, 1);
            });

            
        }
    }

}
