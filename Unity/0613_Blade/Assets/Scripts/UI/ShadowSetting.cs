using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShadowSetting : MonoBehaviour
{
    public Light _Light;
    public Dropdown shadowDropDown;
    void Start()
    {
        Debug.Log(_Light.shadows);
        this.shadowDropDown.onValueChanged.AddListener((a) =>
        {
            this._Light.shadows = (LightShadows)a;
        });
    }
}
