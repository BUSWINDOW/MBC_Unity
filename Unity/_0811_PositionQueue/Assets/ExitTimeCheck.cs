using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTimeCheck : MonoBehaviour
{
    bool isInside = false;
    float time = 0f;
    private void OnTriggerEnter(Collider other)
    {
        isInside = true;
        StartCoroutine(TimeCheck());
    }
    IEnumerator TimeCheck()
    {
        while (isInside)
        {
            yield return null;
            time += Time.deltaTime;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"나오는 시간 : {time}");
        this.isInside = false;
        this.time = 0;
    }
}
