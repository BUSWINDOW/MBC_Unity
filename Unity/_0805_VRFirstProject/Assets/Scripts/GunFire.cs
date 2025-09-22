using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GunFire : MonoBehaviour
{
    XRGrabInteractable grabCtrl;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    void Start()
    {
        this.grabCtrl = GetComponent<XRGrabInteractable>();
        this.grabCtrl.activated.AddListener((args) =>
        {
            var bullet = Instantiate(bulletPrefab);
            bullet.transform.position = bulletSpawnPoint.position;
            //bullet.GetComponent<Rigidbody>().AddForce(bulletSpawnPoint.forward * 1000f, ForceMode.Impulse);
            bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * 10f; // 속도 설정
            Destroy(bullet, 3f); // 3초후 파괴
        });
    }
}
