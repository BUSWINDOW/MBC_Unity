using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerShooter : MonoBehaviourPun
{
    PlayerInput p_Input;
    public GameObject bulletPrefab;
    public ParticleSystem muzzleFlash;
    public Transform firePos;
    void Start()
    {
        this.p_Input = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (this.p_Input.isFire)
            {
                FireBullet();
                photonView.RPC("FireBullet", RpcTarget.Others);
            }
        }
        else
        {

        }
    }
    [PunRPC]
    private void FireBullet()
    {
        if(!this.muzzleFlash.isPlaying)
            this.muzzleFlash.Play();

        GameObject bullet = Instantiate(bulletPrefab, firePos.position, firePos.rotation);
    }
}
