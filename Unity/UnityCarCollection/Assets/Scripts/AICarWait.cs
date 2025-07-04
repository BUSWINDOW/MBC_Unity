using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using UnityEngine;

public class AICarWait : MonoBehaviour
{
    private AICar carCtrl;
    public CinemachineBlendListCamera BlendCam;
    private void Awake()
    {
        this.carCtrl = GetComponent<AICar>();
        this.carCtrl.enabled = false;
        StartCoroutine(this.WaitForSec(() =>
        {

            this.carCtrl.enabled = true;
        }, 1));
        
    }


    IEnumerator WaitForSec(Action act , float sec)
    {
        yield return new WaitForSeconds(sec);
        while (this.BlendCam.LiveChild != this.BlendCam.ChildCameras[1]) //카메라가 다음걸로 바뀌면 이하 실행
        {
            yield return null;
        }
        act();
    }
}
