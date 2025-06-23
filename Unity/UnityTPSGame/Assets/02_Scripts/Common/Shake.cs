using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Shake : MonoBehaviour
{
    //public Transform shakeCamera; // 흔들림 효과를 줄 카메라
    public bool shakeRotate = false; //회전 할 것인지를 판단하는 변수
    private Vector3 originPos = Vector3.zero; // 흔들림 효과 후 원래 위치를 되돌릴 변수
    private Quaternion originRot = Quaternion.identity; // 흔들림 효과 후 원래 회전으로 되돌릴 변수
    void Start()
    {
        //this.transform.DOShakePosition(5);
        //this.transform.DOShakeRotation(5);
        this.originPos = this.transform.position;
        this.originRot = this.transform.rotation;
    }
    public IEnumerator ShakeCamera(
        float duration = 0.05f , 
        float magnitudePos = 0.03f, 
        float magnitudeRot = 0.03f)
    {
        float passTime = 0.0f;
        while (passTime < duration)
        {
            Vector3 shakePos = Random.insideUnitSphere * magnitudePos; // 불규칙한 위치를 추출
            this.transform.position += shakePos;
            if (shakeRotate)
            {
                Vector3 shakeRot = new Vector3(0, 0, Mathf.PerlinNoise(Time.time * magnitudeRot,0));
                this.transform.rotation = Quaternion.Euler(shakeRot);
            }
            passTime += Time.deltaTime; // 시간 누적
            yield return null;
        }

        this.transform.position = originPos;
        this.transform.rotation = originRot;
    }

    public void ShakeTween(float duration)
    {
        this.transform.DOShakePosition(duration);
        if (this.shakeRotate)
            this.transform.DOShakeRotation(duration, new Vector3(0,0,10f));
        
    }
}

