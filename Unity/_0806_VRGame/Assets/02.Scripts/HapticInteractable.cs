using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable]
public class Haptic
{
    [Range(0, 1)] public float intensity = 0;
    public float duration = 0;
}
public class HapticInteractable : MonoBehaviour
{
    public Haptic haptic;
    public Haptic selectHaptic;

    private void Start()
    {
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        interactable.activated.AddListener((args) =>
        {
            SetHaptic(args, haptic); //√— ΩÚ∂ß ¡¯µø
        });
        interactable.selectEntered.AddListener((args) =>
        {
            SetHaptic(args, selectHaptic); // ¿‚¿ª∂ß ¡¯µø
        });
    }

    private void SetHaptic(BaseInteractionEventArgs args, Haptic haptic)
    {
        args.interactorObject.transform.GetComponent<XRBaseController>().SendHapticImpulse(haptic.intensity, haptic.duration);
    }
}