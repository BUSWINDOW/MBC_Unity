using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]

public class BulletCtrl : MonoBehaviour
{

    private void OnEnable()
    {
        GetComponent<Rigidbody>().AddForce(this.transform.forward * 100, ForceMode.Impulse);
        //Destroy(this.gameObject, 5f);
        StartCoroutine(this.WaitSomeSec(() =>
        {
            this.gameObject.SetActive(false);
        }, 5));
    }
    private void OnDisable()
    {
        this.GetComponent<Rigidbody>().Sleep();
    }
    IEnumerator WaitSomeSec(Action action , float sec)
    {
        yield return new WaitForSeconds(sec);
        action();
    }
}
