using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class DOFManager : MonoBehaviour
{
    public DepthOfFieldDeprecated dof;
    public void DepthOfFieldCtrl(bool on_off)
    {
        if (dof != null)
        {
            dof.enabled = on_off;
        }
    }
    public void EnableDepthOfField()
    {
        if (dof != null)
        {
            dof.enabled = true;
        }
    }
    public void DisableDepthOfField()
    {
        if (dof != null)
        {
            dof.enabled = false;
        }
    }
}
