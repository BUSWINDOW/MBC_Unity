using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCube : MonoBehaviour
{
    //통과해서 밖으로 나간걸 체크해야한다
    //그대로 뒤로 돌아간건 체크되면 안된다

    public bool Check { get; private set; }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.Check = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.Check = false;
        }
    }

}
