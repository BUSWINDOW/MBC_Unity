using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableTwoAttach : XRGrabInteractable
{
    public Transform leftAttachTransform;
    public Transform rightAttachTransform;

    Vector3 pos;
    Quaternion rot;
    /*protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.CompareTag("LeftHand"))
        {
            Debug.Log("래프트 핸드");
            attachTransform = leftAttachTransform;
        }
        else if (args.interactableObject.transform.CompareTag("RightHand"))
        {
            Debug.Log("라이트 핸드");
            attachTransform = rightAttachTransform;
        }
        base.OnSelectEntered(args);
    }*/
    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        
        if (args.interactorObject.transform.CompareTag("LeftHand"))
        {
            Debug.Log("래프트 핸드");
            attachTransform = leftAttachTransform;
        }
        else if (args.interactorObject.transform.CompareTag("RightHand"))
        {
            Debug.Log("라이트 핸드");
            attachTransform = rightAttachTransform;
        }
    }
}
