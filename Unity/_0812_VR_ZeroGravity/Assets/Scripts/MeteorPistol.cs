using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;

public class MeteorPistol : MonoBehaviour
{
    ParticleSystem particle;
    XRGrabInteractable grab;

    public LayerMask layerMask;
    public Transform shotSource;
    public float shotDistance = 100f;

    private void Start()
    {
        this.particle = GetComponentInChildren<ParticleSystem>();
        this.grab = GetComponent<XRGrabInteractable>();

        this.grab.activated.AddListener((args) =>
        {
            this.particle.Play();
            StartCoroutine(ShotRayRoutine());
        });
        this.grab.deactivated.AddListener((args) =>
        {
            this.particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            StopCoroutine(ShotRayRoutine());
        });
        this.grab.selectExited.AddListener((args) =>
        {
            StopCoroutine(ShotRayRoutine());
            this.particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        });
    }
    IEnumerator ShotRayRoutine()
    {
        while (true)
        {
            RaycastHit hit;
            if (Physics.Raycast(shotSource.position, shotSource.forward, out hit, shotDistance, layerMask))
            {
                var breakable = hit.collider.GetComponent<Breakable>();
                if (breakable != null)
                {
                    breakable.Break();
                }
            }
            yield return null;
        }
    }
}
