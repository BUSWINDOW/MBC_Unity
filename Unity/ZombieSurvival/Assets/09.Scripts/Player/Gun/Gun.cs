using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum eState
    {
        Ready, // ��� ����
        Fire, // ��� ��
        Reload // ���� ��
    }
    public eState state = eState.Ready; // ���� ���� ����
    public GunData gunData; // �� ������ ��ũ���ͺ� ������Ʈ

    private AudioSource source;
    void Start()
    {
        this.source = GetComponent<AudioSource>();
    }


    void Update()
    {
        
    }
}
