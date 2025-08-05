using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDollCtrl : MonoBehaviour
{
    Rigidbody[] ragDollRb;
    Animator anim;
    void Start()
    {
        this.ragDollRb = GetComponentsInChildren<Rigidbody>();
        this.anim = GetComponent<Animator>();
        SetKinematic(true);

        /*//시간이 지난다음 애니메이션을 끄면서 isKinematic도 같이 체크해제하는 부분
        StartCoroutine(UtilCodes.WaitForSec(() =>
        {
            ActiveRagDoll();
        }, 10f));*/
    }

    public void ActiveRagDoll()
    {
        SetKinematic(false);
        this.anim.enabled = false;
    }

    private void SetKinematic(bool enable)
    {
        foreach (var rb in ragDollRb)
        {
            rb.isKinematic = enable;
        }
    }
}
