using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseCart : MonoBehaviour
{
    List<Transform>NodeList = new List<Transform>();
    public int curNodeIdx = 0;
    void Start()
    {
        var path = GameObject.Find("PathPoints");
        if (path != null)
        {
            path.GetComponentsInChildren<Transform>(NodeList);
            NodeList.RemoveAt(0);
        }
        
    }
    void FixedUpdate()
    {
        WayPointMove();
        CheckDist();
    }
    void WayPointMove()
    {
        Vector3 movePos = NodeList[curNodeIdx].position - this.transform.position;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(movePos), Time.fixedDeltaTime);
        this.transform.Translate(Vector3.forward * 10 * Time.fixedDeltaTime);
    }
    void CheckDist()
    {
        if (Vector3.Distance(this.transform.position, this.NodeList[curNodeIdx].position) <= 2.5f)
        {
            if (this.curNodeIdx++ == this.NodeList.Count - 1)
            { this.curNodeIdx = 0; }
        }
    }
}
