using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(this.transform.forward * 100, ForceMode.Impulse);
        Destroy(this.gameObject, 5f);
    }
}
