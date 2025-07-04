using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum eState
    {
        Ready, // 대기 상태
        Fire, // 사격 중
        Reload // 장전 중
    }
    public eState state = eState.Ready; // 총의 현재 상태
    public GunData gunData; // 총 데이터 스크립터블 오브젝트

    private AudioSource source;
    void Start()
    {
        this.source = GetComponent<AudioSource>();
    }


    void Update()
    {
        
    }
}
