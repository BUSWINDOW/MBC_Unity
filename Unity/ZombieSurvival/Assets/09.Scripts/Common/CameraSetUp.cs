using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class CameraSetUp : MonoBehaviourPun
{
    private void Start()
    {
        if (photonView.IsMine)
        {
            CinemachineVirtualCamera vc = FindObjectOfType<CinemachineVirtualCamera>();
            if (vc != null)
            {
                vc.Follow = this.transform; // 카메라가 플레이어를 따라가도록 설정
                vc.LookAt = this.transform; // 카메라가 플레이어를 바라보도록 설정
            }
        }
    }
}
