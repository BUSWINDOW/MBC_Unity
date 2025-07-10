using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void playSFX(Vector3 pos, AudioClip clip, bool looped)
    {
        GameObject soundObject = new GameObject("SoundEffect");
        soundObject.transform.position = pos;
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.loop = looped;
        audioSource.Play();
        if (!looped)
        {
            Destroy(soundObject, clip.length);
        }
    }
}
