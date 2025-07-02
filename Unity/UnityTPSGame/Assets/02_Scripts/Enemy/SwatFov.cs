using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands.Changelist;
using UnityEngine;

public class SwatFov : MonoBehaviour
{
    public float viewRange = 15f; //적 캐릭터 시야 사정거리
    [Range(0, 360)]
    public float viewAngle = 120f; //적 캐릭터 시야각 범위

    private Transform playerTr;
    private int playerLayer;
    private int obstacleLayer;
    private int layerMask;

    private const string player = "Player";

    void Start()
    {
        this.playerTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        this.playerLayer = LayerMask.NameToLayer("Player");
        this.obstacleLayer = LayerMask.NameToLayer("Object");
        this.layerMask = 1 << this.playerLayer | 1 << this.obstacleLayer;
    }
    public Vector3 CirclePoint(float angle)
    {
        angle += this.transform.eulerAngles.y; // 로컬 좌표계 기준으로 설정 하기 위해 적 캐릭터의 y축 회전값을 더함


        //원형의 좌표를 구하는 공식 : Mathf.Deg2Rad = PI * 2 / 360
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
        //일반 각도를 라디안 각도로 변환
    }
    public bool isTracePlayer()
    {
        bool isTrace = false;
        Collider[] cols = Physics.OverlapSphere(this.transform.position, this.viewRange, 1 << playerLayer); //범위 체크
        if(cols.Length > 0) // 배열의 갯수가 0이 아닐때 플레이어가 범위 안에 있다고 판단
        {
            Vector3 dir = (this.playerTr.position - this.transform.position).normalized;

            if(Vector3.Angle(this.transform.forward , dir) < viewAngle * 0.5f) //시야각에 들어왔는지 판단
                                                                               //위에서 범위에 들어왔는진 판단했으니 각만 판단하면 됨
            {
                isTrace = true;
            }
        }

        return isTrace;
    }

    public bool isViewPlayer()
    {
        bool isView = false;

        RaycastHit hit;
        Vector3 dir = (this.playerTr.position - this.transform.position).normalized;

        if(Physics.Raycast(this.transform.position, dir, out hit, viewRange , layerMask)) //사이에 장애물이 있다면 공격 하지 않도록
        {
            isView = hit.collider.CompareTag(player);
        }

        return isView;
    }
}
