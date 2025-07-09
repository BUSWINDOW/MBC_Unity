using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonCtrl : MonoBehaviour
{
    private TankInput input;
    public float rotationSpeed = 10f; // 회전 속도

    // 회전 각도를 제한하기 위한 변수
    private float maxRotation = -38f; // 제일 위에 있을 때
    private float minRotation = 9f; // 제일 아래에 있을 때
    private float currentRotation = 0f; // 현재 회전 각도

    void Start()
    {
        this.input = GetComponentInParent<TankInput>();
    }
    void Update()
    {
        var rotate =-this.input.m_scroll * this.rotationSpeed;
        //this.transform.Rotate(Vector3.right * rotate);
        //회전 정도를 제한해야함
        currentRotation += rotate;
        //9~ -38 까지 움직임

        if (currentRotation < maxRotation) //음수여야 위로 올라감
                                           //x축을 오른쪽으로 돌리면(양수) 내려가니까 음수여야 위로 올라감
        {
            currentRotation = maxRotation;
        }
        else if (currentRotation > minRotation)
        {
            currentRotation = minRotation;
        }
        this.transform.rotation = Quaternion.Euler(currentRotation, this.transform.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z);

    }
}
