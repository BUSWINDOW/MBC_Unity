using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class UIManager : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    private GameObject player;
    void Start()
    {
        this.player = GameObject.FindWithTag("Player");
        var snap = this.player.GetComponent<ActionBasedSnapTurnProvider>();
        var continuous = this.player.GetComponent<ActionBasedContinuousTurnProvider>();
        this.dropdown.onValueChanged.AddListener((value) =>
        {
            switch (value)
            {
                case 0:
                    {
                        snap.enabled = false;
                        continuous.enabled = true;
                        break;
                    }
                case 1:
                    {
                        snap.enabled = true;
                        continuous.enabled = false;
                        break;
                    }
            }
        });
    }

    void Update()
    {
        
    }
}
