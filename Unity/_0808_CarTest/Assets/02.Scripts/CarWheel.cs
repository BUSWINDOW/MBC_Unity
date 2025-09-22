using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWheel : MonoBehaviour
{
    public WheelCollider targetWheel; // 휠 콜라이더
    public Vector3 WheelPos = Vector3.zero; // 휠 위치
    public Quaternion WheelRot = Quaternion.identity; // 휠 회전

    void LateUpdate()
    {
        targetWheel.GetWorldPose(out WheelPos, out WheelRot);
        // 휠 콜라이더의 월드 위치와 회전을 가져옵니다.
        transform.position = WheelPos; // 휠 모델의 위치를 업데이트
        transform.rotation = WheelRot; // 휠 모델의 회전을 업데이트

    }
}
