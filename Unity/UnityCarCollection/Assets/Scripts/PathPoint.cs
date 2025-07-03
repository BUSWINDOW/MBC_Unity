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
            Vector3 curNode = this.NodeList[i].position; //현재 노드
            Vector3 prevNode = Vector3.zero; //이전 노드
            if(i > 0)
            {
                prevNode = NodeList[i-1].position;
            }
            else if(i == 0 && NodeList.Count > 1) // 분명 2개 이상 들어가있는데 첫번째 노드라면
            {
                prevNode = this.NodeList[NodeList.Count - 1].position; // 이전 노드는 마지막 노드
            }
            Gizmos.DrawLine(prevNode, curNode); // 이전거랑 다음거랑 선으로 그음
            Gizmos.DrawSphere(curNode, 0.8f); // 현재위치에 구체 그리기
        }

    }
}
