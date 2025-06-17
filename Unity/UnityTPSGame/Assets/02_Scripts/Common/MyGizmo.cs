using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmo : MonoBehaviour
{
    public float _radius = 0.5f;
    public Color _color = Color.red;
    private void OnDrawGizmos()
    {
        Gizmos.color = _color;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
