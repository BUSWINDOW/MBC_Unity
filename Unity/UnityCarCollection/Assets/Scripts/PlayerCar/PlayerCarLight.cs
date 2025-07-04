using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarLight : MonoBehaviour
{
    [SerializeField] private GameObject flashLightBundle;
    [SerializeField] private GameObject backLightBundle;
    [SerializeField] private Light[] backLights;

    public void FlashTurnOn_Off(bool? turn = null)
    {
        if (turn == null)
            this.flashLightBundle.SetActive(!this.flashLightBundle.activeSelf);
        else
        {
            this.flashLightBundle.SetActive(false);
        }
    }
    public void BackLightCtrl(bool turn, Color color = new Color())
    {
        this.backLightBundle.SetActive(turn);
        if (!turn)
        {
            return;
        }
        foreach (Light light in backLights) 
        {
            light.color = color;
        }
    }
}
