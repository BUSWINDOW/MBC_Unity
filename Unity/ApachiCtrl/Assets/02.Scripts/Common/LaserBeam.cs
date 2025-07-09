using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float maxDistance = 100f;

    void Start()
    {
        this.lineRenderer = this.GetComponent<LineRenderer>();
        this.lineRenderer.useWorldSpace = false;
        this.lineRenderer.positionCount = 2;
        this.lineRenderer.enabled = false;
    }
    public void FireRay()
    {
        Ray ray = new Ray(this.transform.position + (Vector3.up * 0.02f), this.transform.forward);
        RaycastHit hit;
        this.lineRenderer.SetPosition(0, this.transform.InverseTransformPoint(ray.origin)); // 월드 좌표를 로컬 좌표로 변환

        if(Physics.Raycast(ray,out hit, this.maxDistance , 1 << 6))
        {
            this.lineRenderer.SetPosition(1, this.transform.InverseTransformPoint(hit.point)); // 월드 좌표를 로컬 좌표로 변환
            //맞은 좌표를 끝점으로
        }
        else //만약 안맞으면
        {
            this.lineRenderer.SetPosition(1, this.transform.InverseTransformPoint(ray.GetPoint(maxDistance))); // 최대 거리로 끝점 설정
        }
        StartCoroutine(ShowLaserBeam());
    }
    //WaitForSeconds ws = new WaitForSeconds(0.1f);
    IEnumerator ShowLaserBeam()
    {
        this.lineRenderer.enabled = true;
        yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        this.lineRenderer.enabled = false;
    }
}
