using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour
{
    public Color lineColor = Color.black;
    public List<Transform> NodeList;
    private void OnDrawGizmos()
    {
        Gizmos.color = lineColor;
        this.NodeList = new List<Transform>();
        Transform[] PathTr = GetComponentsInChildren<Transform>();
        for(int i = 0; i<PathTr.Length; i++)
        {
            if (PathTr[i] != this.transform)
            {
                this.NodeList.Add(PathTr[i]);
            }
        }
        for(int i = 0; i< this.NodeList.Count; i++)
        {
            Vector3 curNode = this.NodeList[i].position; //���� ���
            Vector3 prevNode = Vector3.zero; //���� ���
            if(i > 0)
            {
                prevNode = NodeList[i-1].position;
            }
            else if(i == 0 && NodeList.Count > 1) // �и� 2�� �̻� ���ִµ� ù��° �����
            {
                prevNode = this.NodeList[NodeList.Count - 1].position; // ���� ���� ������ ���
            }
            Gizmos.DrawLine(prevNode, curNode); // �����Ŷ� �����Ŷ� ������ ����
            Gizmos.DrawSphere(curNode, 0.8f); // ������ġ�� ��ü �׸���
        }

    }
}
