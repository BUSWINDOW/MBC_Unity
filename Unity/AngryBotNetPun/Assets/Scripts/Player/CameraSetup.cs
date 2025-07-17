using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class CameraSetup : MonoBehaviourPun
{
    CinemachineVirtualCamera vc;
    void Start()
    {
        if (!photonView.IsMine) return;
        this.vc = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        this.vc.m_Follow = this.transform;
        this.vc.m_LookAt = this.transform;
        
    }
}
