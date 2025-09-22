using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoints : MonoBehaviour
{
    [SerializeField]private List<Transform> pathPoints;
    private void Start()
    {
        pathPoints = new List<Transform>();
        this.GetComponent<Transform>().GetComponentsInChildren<Transform>(pathPoints);
        this.pathPoints.RemoveAt(0); // 자기자신 지우기
    }
    public void GetNextPoint(ref int idx)
    {
        if (++idx == this.pathPoints.Count)
            idx = 0;
    }
    public Vector3 GetCurrentPoint(int idx)
    {
        return pathPoints[idx].position;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        foreach (Transform child in transform)
        {
            Gizmos.DrawSphere(child.position, 0.1f);
        }
        Gizmos.color = Color.green;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Vector3 start = transform.GetChild(i).position;
            Vector3 end = transform.GetChild(i + 1).position;
            Gizmos.DrawLine(start, end);
        }

        Gizmos.DrawLine
            (transform.GetChild(transform.childCount - 1).position,
                         transform.GetChild(0).position
            );
        
    }

}
