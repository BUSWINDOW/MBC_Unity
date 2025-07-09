using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApacheCtrl : MonoBehaviour
{
    public Rigidbody rb; // Rigidbody 컴포넌트 변수 선언
    public float moveSpeed = 0f; // 비행 속도 변수 선언
    public float rotateSpeed = 0f; // 회전 속도 변수 선언
    public float verticalSpeed = 0f; // 수직 속도 변수 선언

    float curRot = 0;
    void Start()
    {
    }
    void FixedUpdate() // 키보드 입력값만큼 이동하기 위한 로직
    {
        #region 좌우로 회전 하는 로직
        if (Input.GetKey(KeyCode.A))
        {
            this.rotateSpeed -= 0.02f; // 왼쪽 방향키를 누르면 왼쪽으로 회전
        }
        else if (Input.GetKey(KeyCode.D))
        {
            this.rotateSpeed += 0.02f; // 오른쪽 방향키를 누르면 오른쪽으로 회전
        }
        else if (this.rotateSpeed != 0)
        {
            this.rotateSpeed += this.rotateSpeed != 0 ? this.rotateSpeed > 0f ? -0.02f : 0.02f : 0; // 회전 속도가 0이 되도록 감속
        }
        this.transform.Rotate(0f, this.rotateSpeed, 0f); // 현재 회전 속도만큼 회전
        #endregion

        #region 앞뒤로 이동 하는 로직
        if (Input.GetKey(KeyCode.W))
        {
            this.moveSpeed += 0.02f; // 앞쪽 방향키를 누르면 앞으로 이동
        }
        else if (Input.GetKey(KeyCode.S))
        {
            this.moveSpeed -= 0.02f; // 뒤쪽 방향키를 누르면 뒤로 이동
        }
        else if (this.moveSpeed != 0)
        {
            this.moveSpeed += this.moveSpeed != 0 ? this.moveSpeed > 0f ? -0.02f : 0.02f : 0;
        }
        this.transform.Translate(Vector3.forward * this.moveSpeed * Time.deltaTime, Space.Self);
        #endregion

        #region 수직으로 이동 하는 로직
        if (Input.GetKey(KeyCode.Space))
        {
            this.verticalSpeed += 0.02f; // 스페이스바를 누르면 공중에 뜸
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            this.verticalSpeed -= 0.02f; // alt누르면 착지
        }
        else if (this.verticalSpeed != 0)
        {
            this.verticalSpeed += this.verticalSpeed != 0 ? this.verticalSpeed > 0f ? -0.02f : 0.02f : 0;
        }
        this.transform.Translate(Vector3.up * this.verticalSpeed * Time.deltaTime, Space.World);
        #endregion

        #region 마우스 휠로 수평 조절하는 로직
        var rotate = Input.GetAxisRaw("Mouse ScrollWheel") * 10; // 마우스 휠 입력값을 가져옴
        
        curRot += rotate;

        this.transform.rotation = Quaternion.Euler(curRot, this.transform.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z);

        #endregion

    }
}