using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTiedTrash : MonoBehaviour
{
    private Transform grassObj;
    private Transform trashObj;

    public float grassHeight;

    private void Update()
    {
        this.Init(); // Å×½ºÆ®
    }
    public void Init()
    {
        this.grassObj = this.transform.GetChild(0);
        this.trashObj = this.transform.GetChild(1);

        this.grassObj.localScale = new Vector3(1, this.grassHeight, 1);
        this.trashObj.localPosition = new Vector3(0, this.grassHeight, 0);
    }
}
