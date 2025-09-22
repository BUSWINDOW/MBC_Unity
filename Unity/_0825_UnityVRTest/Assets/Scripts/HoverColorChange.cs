using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HoverColorChange : MonoBehaviour
{
    XRSimpleInteractable interactable;
    public Color hoverColor = Color.white;
    public Color defaultColor = Color.red;

    MeshRenderer meshRenderer;
    private void Start()
    {
        this.interactable = GetComponent<XRSimpleInteractable>();
        this.meshRenderer = GetComponent<MeshRenderer>();

        this.interactable.firstHoverEntered.AddListener((args) =>
        {
            this.meshRenderer.material.color = hoverColor;
        });
        this.interactable.lastHoverExited.AddListener((args) =>
        {
            this.meshRenderer.material.color = defaultColor;
        });
    }
}
