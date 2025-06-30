using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RifleGunData" , menuName ="ScriptableObjects/RifleGunData" , order =1)]
// 파일명,  메뉴 이름,   순서
public class PlayerRifleData : ScriptableObject
{
    public AudioClip shotClip;
    public AudioClip reloadClip;
    public float fireRate = 0.1f; // 발사 속도
    public float reloadTime = 2.0f; // 재장전 시간
}
