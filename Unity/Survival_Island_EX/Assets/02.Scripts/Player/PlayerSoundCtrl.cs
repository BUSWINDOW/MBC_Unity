using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundCtrl : MonoBehaviour
{
    AudioSource source;

    public AudioClip shootSound;
    public AudioClip reloadSound;

    void Start()
    {
        this.source = this.transform.GetChild(0).GetChild(0).GetComponent<AudioSource>();
    }
    public void PlaySound(AudioClip clip)
    {
        this.source.PlayOneShot(clip);
    }
}
