using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TurnChange : MonoBehaviour
{
    ActionBasedSnapTurnProvider snapTurnProvider;
    ActionBasedContinuousTurnProvider continuousTurnProvider;
    private void Start()
    {
        this.snapTurnProvider = GetComponent<ActionBasedSnapTurnProvider>();
        this.continuousTurnProvider = GetComponent<ActionBasedContinuousTurnProvider>();
    }

    public void TurnChangeMethod(int menu)
    {
        this.snapTurnProvider.enabled = menu == 1;
        this.continuousTurnProvider.enabled = menu == 0;
    }
}
