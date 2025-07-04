using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarRide : MonoBehaviour
{
    private PlayerCar carCtrl;
    private readonly string playerTag = "Player";
    private GameObject rider;
    void Start()
    {
        this.carCtrl = GetComponent<PlayerCar>();
        this.carCtrl.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(this.playerTag))
        {
            this.rider = other.gameObject;
            this.carCtrl.enabled = true;
            rider.SetActive(false);
            rider.transform.SetParent(this.transform, false);
            this.rider.transform.position += Vector3.right * 4;
        }
    }
    public void GetOffCar()
    {
        rider.transform.SetParent(null);
        this.rider.SetActive(true);
        
    }
}
