using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RifleGunData" , menuName ="ScriptableObjects/RifleGunData" , order =1)]
// ���ϸ�,  �޴� �̸�,   ����
public class PlayerRifleData : ScriptableObject
{
    public AudioClip shotClip;
    public AudioClip reloadClip;
    public float fireRate = 0.1f; // �߻� �ӵ�
    public float reloadTime = 2.0f; // ������ �ð�
}
