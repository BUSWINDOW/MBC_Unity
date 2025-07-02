using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class FollowCamera : MonoBehaviour
{
    public Transform target;

    [SerializeField] float moveDamping = 15f;
    [SerializeField] float rotateDamping = 10f;
    [SerializeField] float distance = 5f;
    [SerializeField] float height = 4f;
    [SerializeField] float targetOffset = 2f;

    private const string playerTag = "Player";

    public float MaxHeight = 15f;
    public float castOffset = 1; // 플레이어에 투사할 레이캐스트의 높이 오프셋
    public float originHeight;

    //public CinemachineVirtualCamera vc;
    
    void Start()
    {
        this.originHeight = this.height;
    }

    void LateUpdate() //Update가 먼저 실행되고 그 이후에 실행 됨
    {
        //Debug.Log("Late");
        var CamPos = this.target.position - (Vector3.forward * distance) + (Vector3.up * height);
        this.transform.position = Vector3.Slerp(this.transform.position, CamPos, Time.deltaTime * moveDamping);


        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, this.target.rotation, Time.deltaTime * rotateDamping);
        this.transform.LookAt(this.target.position + (Vector3.up * this.targetOffset));
    }
    private void FixedUpdate()
    {
        //Debug.Log("Fixed");

    }
    private void Update()
    {
        //Debug.Log("Normal");
        Vector3 castTarget = target.position + (Vector3.up * castOffset);
        Vector3 castDir = (castTarget - this.transform.position) .normalized ;

        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, castDir, out hit, Mathf.Infinity))
        {
            if (!hit.collider.CompareTag(playerTag)) // 만약 닿은게 플레이어가 아니라면
            {
                this.height = Mathf.Lerp(height, MaxHeight, Time.deltaTime * 10);
            }
            else // 플레이어가 맞으면
            {
                this.height = Mathf.Lerp(height, originHeight, Time.deltaTime * 10);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(target.position + (Vector3.up * targetOffset),0.1f);
        Gizmos.DrawLine(target.position + Vector3.up * targetOffset, transform.position);
    }
}
