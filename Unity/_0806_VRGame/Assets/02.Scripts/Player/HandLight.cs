using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandLight : MonoBehaviour
{
    private Light handLight;
    void Start()
    {
        this.handLight = GetComponent<Light>();
    }
    void Update()
    {
        Light_On_Off();
    }

    private void Light_On_Off()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            this.handLight.enabled = !this.handLight.enabled;
        }
    }
}
