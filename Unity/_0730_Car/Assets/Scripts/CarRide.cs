using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CarRide : MonoBehaviour
{
    [SerializeField] CinemachineBlendListCamera rideAnimationCam;
    [SerializeField] CinemachineVirtualCamera carFollowCam;
    [SerializeField] Animator anim;
    private readonly int hashDoorOpen = Animator.StringToHash("doorOpen");
    private readonly string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        other.gameObject.SetActive(false);
        other.transform.SetParent(this.transform);
        this.rideAnimationCam.gameObject.SetActive(true);
        this.anim.SetTrigger(hashDoorOpen);
        StartCoroutine(UtilScript.WaitForSeconds(() =>
        {
            this.rideAnimationCam.gameObject.SetActive(false);
            this.carFollowCam.gameObject.SetActive(true);
            this.GetComponent<PlayerCar>().enabled = true;
        }, 3.5f));
    }
}
