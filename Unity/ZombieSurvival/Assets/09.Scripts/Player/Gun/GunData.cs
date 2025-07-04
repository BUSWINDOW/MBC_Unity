using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "GunData", menuName = "ScriptableObjects/GunData")]
public class GunData : ScriptableObject
{
    public AudioClip shotClip; // �Ѿ� �߻� ����
    public AudioClip reloadClip; // ���� ����
    public int damage = 25; // ���� ������
    public int magCapacity = 25; // źâ �뷮
    public int startAmmo = 100; // ���� ź�� ��
    public float timeBetweenShots = 0.1f; // ���� �ӵ�
    public float reloadTime = 1.8f; // ���� �ð�
    // �߰����� �� �����Ͱ� �ʿ��� ��� ���⿡ �߰��� �� �ֽ��ϴ�.
}
