using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public GameObject hitEffect;
    public int actorNum;
    void Start()
    {
        this.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 1000f);
        // 상대적 전방으로 힘을 가함
        // = this.GetComponent<Rigidbody>().AddForce(this.transform.forward);
        Destroy(this.gameObject, 3.0f);
    }
    private void OnCollisionEnter(Collision col)
    {
        var contact = col.GetContact(0);

        var eff = Instantiate(this.hitEffect, contact.point, Quaternion.LookRotation(-contact.normal));

        Destroy(eff, 2.0f);
        Destroy(this.gameObject);
    }
}
