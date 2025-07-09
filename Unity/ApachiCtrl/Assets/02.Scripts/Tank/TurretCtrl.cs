using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//마우스 x축 회전에 맞춰서 회전함

public class TurretCtrl : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;

    void Start()
    {
        
    }

    void Update()
    {
        this.ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(this.ray.origin, this.ray.direction * 100f, Color.red);
        if (Physics.Raycast(this.ray, out this.hit, 100f , 1 << 6))
        {
            Vector3 target = this.hit.point;
            target.y = this.transform.position.y; // y축은 고정
            Vector3 direction = target - this.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, Time.deltaTime * 10f);
        }
    }
}
