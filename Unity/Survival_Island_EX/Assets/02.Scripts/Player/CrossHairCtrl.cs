using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHairCtrl : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    private Image aimImage;
    private bool isGaze;
    void Start()
    {
        this.isGaze = false;
        this.aimImage = this.transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }
    void Update()
    {
        this.ray = new Ray(this.transform.GetChild(0).position, this.transform.GetChild(0).forward*100);
        if(Physics.Raycast(this.ray,out this.hit, 100, 1 << 6))
        {
            this.aimImage.color = Color.red;
            if (!this.isGaze)
            {
                Debug.Log("Start");
                StartCoroutine(this.GazeRoutine());
            }
            this.isGaze = true;
        }
        else
        {
            StopAllCoroutines();
            this.aimImage.color = Color.white;
            this.aimImage.transform.localScale = Vector3.one * 0.4f;
            this.isGaze=false;
        }
    }
    IEnumerator GazeRoutine()
    {
        float sum = 0;
        while (true)
        {
            sum += Time.deltaTime;
            this.aimImage.transform.localScale = Vector3.one * Mathf.Lerp(0.4f, 0.5f, sum / 0.3f);
            yield return null;
        }
    }
}
