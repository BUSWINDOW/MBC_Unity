using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandLight : MonoBehaviour
{
    Transform redLight;
    Transform greenLight;
    Transform blueLight;
    public AudioClip turnSound;
    void Start()
    {
        this.redLight = this.transform.GetChild(0);
        this.greenLight = this.transform.GetChild(1);
        this.blueLight = this.transform.GetChild(2);

        StartCoroutine(this.LightOnOffRoutine(this.redLight));
        StartCoroutine(this.LightOnOffRoutine(this.blueLight));
        StartCoroutine(this.LightOnOffRoutine(this.greenLight));
    }
    IEnumerator LightOnOffRoutine(Transform lightInfo)
    {
        Light light = lightInfo.GetComponent<Light>();
        AudioSource source = lightInfo.GetComponent<AudioSource>();
        while (true) 
        {
            yield return new WaitForSeconds(Random.Range((float)0,(float)1));
            
            light.enabled = !light.enabled;
            source.PlayOneShot(this.turnSound);
        }
        
    }
}
