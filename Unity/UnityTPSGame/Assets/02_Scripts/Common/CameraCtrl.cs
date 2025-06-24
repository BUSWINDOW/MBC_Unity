using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


public class CameraCtrl : MonoBehaviour
{
    CinemachineVirtualCamera vc;
    CinemachineBasicMultiChannelPerlin noise;
    private void Awake()
    {
        BarrelCtrl.shakeAction += this.ShakeCamera;
    }
    void Start()
    {
        vc = this.GetComponent<CinemachineVirtualCamera>();
        this.noise = vc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        this.StopCameraShake();
    }
    public void StopCameraShake()
    {
        noise.m_AmplitudeGain = 0.0f;
        noise.m_FrequencyGain = 0.0f;
    }
    public void ShakeCamera()
    {
        noise.m_AmplitudeGain = 5.0f;
        noise.m_FrequencyGain = 3.0f;
        StartCoroutine(WaitRunMethod(this.StopCameraShake, 1.5f));
        StartCoroutine(WaitRunMethod(this.StopCameraShake, 1.5f));
    }
    IEnumerator WaitRunMethod(Action action, float sec)
    {
        yield return new WaitForSeconds(sec);
        action();
    }
}
