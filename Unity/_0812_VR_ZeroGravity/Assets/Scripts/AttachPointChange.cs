using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabAttachPointChangeInteractable : XRGrabInteractable
{

    Transform leftAttach;
    Transform rightAttach;
    // Start is called before the first frame update
    void Start()
    {
        this.rightAttach = this.attachTransform;
        this.leftAttach = this.secondaryAttachTransform;
    }
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        
        if (args.interactorObject.transform.CompareTag("LeftHand"))
        {
            this.attachTransform = this.leftAttach;
        }
        else if (args.interactorObject.transform.CompareTag("RightHand"))
        {
            this.attachTransform = this.rightAttach;
        }
        base.OnSelectEntering(args);
    }
}
