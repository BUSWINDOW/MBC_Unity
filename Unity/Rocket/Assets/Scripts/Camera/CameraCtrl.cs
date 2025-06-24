using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera vc;
    [SerializeField] CinemachineBasicMultiChannelPerlin noise;
    void Start()
    {
        this.vc = this.GetComponent<CinemachineVirtualCamera>();
        this.noise = this.vc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        Rocket.onHitAction += this.CameraShake;
        this.StopCameraShake();
    }
    private void CameraShake()
    {
        this.noise.m_AmplitudeGain = 3;
        this.noise.m_FrequencyGain = 1;
        StartCoroutine(this.WaitSomeSec(StopCameraShake, 0.3f));
    }
    private void StopCameraShake()
    {
        this.noise.m_AmplitudeGain = 0;
        this.noise.m_FrequencyGain = 0;
    }
    IEnumerator WaitSomeSec(Action action, float sec)
    {
        yield return new WaitForSeconds(sec);
        action();
    }
}
