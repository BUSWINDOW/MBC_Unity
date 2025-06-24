using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;

    [SerializeField] float moveDamping = 15f;
    [SerializeField] float rotateDamping = 10f;
    [SerializeField] float distance = 5f;
    [SerializeField] float height = 4f;
    [SerializeField] float targetOffset = 2f;
    
    void Start()
    {
        
    }

    void LateUpdate() //Update가 먼저 실행되고 그 이후에 실행 됨
    {
        Debug.Log("Late");
        var CamPos = this.target.position - (Vector3.forward * distance) + (Vector3.up * height);
        this.transform.position = Vector3.Slerp(this.transform.position, CamPos, Time.deltaTime * moveDamping);


        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, this.target.rotation, Time.deltaTime * rotateDamping);
        this.transform.LookAt(this.target.position + (Vector3.up * this.targetOffset));
    }
    private void FixedUpdate()
    {
        Debug.Log("Fixed");

    }
    private void Update()
    {
        Debug.Log("Normal");

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(target.position + (Vector3.up * targetOffset),0.1f);
        Gizmos.DrawLine(target.position + Vector3.up * targetOffset, transform.position);
    }
}
