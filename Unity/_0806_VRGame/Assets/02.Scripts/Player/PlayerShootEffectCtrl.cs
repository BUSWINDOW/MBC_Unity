using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShootEffectCtrl : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem shootEffect;
    [SerializeField]
    private ParticleSystem bulletEffect;
    public void PlayEffect() 
    {
        this.shootEffect.Play();
        this.bulletEffect.Play();
    }
}
