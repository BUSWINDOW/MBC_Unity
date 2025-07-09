using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TrackAnim : MonoBehaviour
{
    private float speed = 1;
    private MeshRenderer meshRenderer;
    private TankInput input;
    void Start()
    {
        this.meshRenderer = GetComponent<MeshRenderer>();
        this.input = GetComponentInParent<TankInput>();
    }

    void Update()
    {

        if (this.input != null)
        {
            var offset = Time.time * speed * this.input.v;
            this.meshRenderer.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
            this.meshRenderer.material.SetTextureOffset("_BumpMap", new Vector2(0, offset));
        }
        
    }
}
