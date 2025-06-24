using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
public class Aseteroid : MonoBehaviour
{
    public float Speed;
    private Transform tr;
    private readonly string coinTag = "COIN"; //���� �±�
    public TrailRenderer trail;

    void Start()
    {
        Speed = Random.Range(20f, 35f); //�������� �ӵ� ����
        tr = GetComponent<Transform>();
        this.trail = this.GetComponent<TrailRenderer>();
    }
    void Update()
    {                  //���� * �ӵ� = Velocity
        tr.Translate(Vector3.left * Speed * Time.deltaTime); //�������� �̵�
        if(tr.position.x <=-10f)
            //Destroy(this.gameObject); 
            this.gameObject.SetActive(false);

    }
    private void OnDisable()
    {
        this.trail.Clear();
    }

}
