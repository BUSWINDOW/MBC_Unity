using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

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
            if (EventSystem.current.IsPointerOverGameObject()) return; // ¿Ã∫•∆Æ »≈
            if (this.p_Input.isFire)
            {
                FireBullet(photonView.OwnerActorNr);
                photonView.RPC("FireBullet", RpcTarget.Others , photonView.OwnerActorNr);
            }
        }
        else
        {

        }
    }
    [PunRPC]
    private void FireBullet(int actorNum)
    {
        if(!this.muzzleFlash.isPlaying)
            this.muzzleFlash.Play();

        GameObject bullet = Instantiate(bulletPrefab, firePos.position, firePos.rotation);
        bullet.GetComponent<BulletCtrl>().actorNum = actorNum;
    }
}
