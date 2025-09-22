using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GunFire : MonoBehaviour
{
    XRGrabInteractable grabInteractable;
    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField]
    Transform firePos;
    void Start()
    {
        this.grabInteractable = GetComponent<XRGrabInteractable>();
        this.grabInteractable.activated.AddListener((args) =>
        {
            var bullet = Instantiate(bulletPrefab, this.firePos.position, this.firePos.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(firePos.forward * 100f); // Adjust force as needed
            Destroy(bullet, 2f); // Destroy the bullet after 2 seconds
        });
    }

}
