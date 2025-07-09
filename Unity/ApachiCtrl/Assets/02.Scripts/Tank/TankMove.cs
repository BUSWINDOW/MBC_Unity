using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMove : MonoBehaviour
{
    public TankInput input;
    public Rigidbody rb;
    void Start()
    {
        this.input = GetComponent<TankInput>();
        this.rb = GetComponent<Rigidbody>();
        this.rb.centerOfMass = new Vector3(0,-0.5f,0); // 물리 엔진에서 탱크의 중심을 조정
    }

    void Update()
    {
        #region 탱크 회전
        this.transform.Rotate(0, input.h * 2f, 0, Space.Self);
        #endregion

        #region 탱크 이동
        this.transform.Translate(Vector3.forward * this.input.v * Time.deltaTime * 100);
        #endregion
    }
}
