using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmo : MonoBehaviour
{
    public enum eType
    {
        Normal, WayPoint, SpawnPoint
    }

    private readonly string wayPointFile = "Skul";
    public eType type = eType.Normal;

    public float _radius = 0.5f;
    public Color _color = Color.red;
    private void OnDrawGizmos()
    {
        if (this.type == eType.Normal)
        {
            Gizmos.color = _color;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
        else
        {
            Gizmos.color = _color;
            Gizmos.DrawWireSphere(transform.position, _radius);
            Gizmos.DrawIcon(this.transform.position + Vector3.up * 1,wayPointFile,true);
        }
        
    }
}
