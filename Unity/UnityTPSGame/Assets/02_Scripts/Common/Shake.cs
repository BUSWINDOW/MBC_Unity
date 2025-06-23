using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Shake : MonoBehaviour
{
    //public Transform shakeCamera; // ��鸲 ȿ���� �� ī�޶�
    public bool shakeRotate = false; //ȸ�� �� �������� �Ǵ��ϴ� ����
    private Vector3 originPos = Vector3.zero; // ��鸲 ȿ�� �� ���� ��ġ�� �ǵ��� ����
    private Quaternion originRot = Quaternion.identity; // ��鸲 ȿ�� �� ���� ȸ������ �ǵ��� ����
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
            Vector3 shakePos = Random.insideUnitSphere * magnitudePos; // �ұ�Ģ�� ��ġ�� ����
            this.transform.position += shakePos;
            if (shakeRotate)
            {
                Vector3 shakeRot = new Vector3(0, 0, Mathf.PerlinNoise(Time.time * magnitudeRot,0));
                this.transform.rotation = Quaternion.Euler(shakeRot);
            }
            passTime += Time.deltaTime; // �ð� ����
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

