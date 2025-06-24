using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
public class Aseteroid : MonoBehaviour
{
    public float Speed;
    private Transform tr;
    private readonly string coinTag = "COIN"; //코인 태그
    public TrailRenderer trail;

    void Start()
    {
        Speed = Random.Range(20f, 35f); //랜덤으로 속도 설정
        tr = GetComponent<Transform>();
        this.trail = this.GetComponent<TrailRenderer>();
    }
    void Update()
    {                  //방향 * 속도 = Velocity
        tr.Translate(Vector3.left * Speed * Time.deltaTime); //왼쪽으로 이동
        if(tr.position.x <=-10f)
            //Destroy(this.gameObject); 
            this.gameObject.SetActive(false);

    }
    private void OnDisable()
    {
        this.trail.Clear();
    }

}
