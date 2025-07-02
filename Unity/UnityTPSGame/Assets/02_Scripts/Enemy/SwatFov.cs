using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands.Changelist;
using UnityEngine;

public class SwatFov : MonoBehaviour
{
    public float viewRange = 15f; //�� ĳ���� �þ� �����Ÿ�
    [Range(0, 360)]
    public float viewAngle = 120f; //�� ĳ���� �þ߰� ����

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
        angle += this.transform.eulerAngles.y; // ���� ��ǥ�� �������� ���� �ϱ� ���� �� ĳ������ y�� ȸ������ ����


        //������ ��ǥ�� ���ϴ� ���� : Mathf.Deg2Rad = PI * 2 / 360
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
        //�Ϲ� ������ ���� ������ ��ȯ
    }
    public bool isTracePlayer()
    {
        bool isTrace = false;
        Collider[] cols = Physics.OverlapSphere(this.transform.position, this.viewRange, 1 << playerLayer); //���� üũ
        if(cols.Length > 0) // �迭�� ������ 0�� �ƴҶ� �÷��̾ ���� �ȿ� �ִٰ� �Ǵ�
        {
            Vector3 dir = (this.playerTr.position - this.transform.position).normalized;

            if(Vector3.Angle(this.transform.forward , dir) < viewAngle * 0.5f) //�þ߰��� ���Դ��� �Ǵ�
                                                                               //������ ������ ���Դ��� �Ǵ������� ���� �Ǵ��ϸ� ��
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

        if(Physics.Raycast(this.transform.position, dir, out hit, viewRange , layerMask)) //���̿� ��ֹ��� �ִٸ� ���� ���� �ʵ���
        {
            isView = hit.collider.CompareTag(player);
        }

        return isView;
    }
}
