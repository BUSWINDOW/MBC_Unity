using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Transform canvasTr;
    public Transform cameraTr;


    void Start()
    {
        canvasTr = this.transform;
        cameraTr = Camera.main.transform;
    }
    void Update()
    {
        canvasTr.LookAt(cameraTr); //캔버스가 카메라를 바라봄
    }
}
