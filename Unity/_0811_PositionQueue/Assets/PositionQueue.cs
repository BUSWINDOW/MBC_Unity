using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PositionQueue : MonoBehaviour
{
    public List<Vector3> posList = new List<Vector3>();
    //실제로 쓸땐 List말고 Queue 사용
    void Start()
    {
        StartCoroutine(CheckPos());
    }

    WaitForSeconds wsForCheckPos = new WaitForSeconds(0.1f);
    IEnumerator CheckPos()
    {
        while (true)
        {
            yield return wsForCheckPos;
            if (this.posList.Count > 1)
            {
                this.posList.RemoveAt(0);
            }
            this.posList.Add(this.transform.position);
            
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"현재 속도 : {10 * Vector3.Distance(this.transform.position, this.posList[0])}m/s" +
            $"{this.posList[0]} , {this.transform.position}");
    }
}
