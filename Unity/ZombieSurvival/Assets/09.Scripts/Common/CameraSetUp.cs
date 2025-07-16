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
                vc.Follow = this.transform; // ī�޶� �÷��̾ ���󰡵��� ����
                vc.LookAt = this.transform; // ī�޶� �÷��̾ �ٶ󺸵��� ����
            }
        }
    }
}
