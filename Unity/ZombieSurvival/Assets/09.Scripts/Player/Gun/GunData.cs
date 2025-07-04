using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "GunData", menuName = "ScriptableObjects/GunData")]
public class GunData : ScriptableObject
{
    public AudioClip shotClip; // 총알 발사 사운드
    public AudioClip reloadClip; // 장전 사운드
    public int damage = 25; // 총의 데미지
    public int magCapacity = 25; // 탄창 용량
    public int startAmmo = 100; // 시작 탄약 수
    public float timeBetweenShots = 0.1f; // 연사 속도
    public float reloadTime = 1.8f; // 장전 시간
    // 추가적인 총 데이터가 필요할 경우 여기에 추가할 수 있습니다.
}
