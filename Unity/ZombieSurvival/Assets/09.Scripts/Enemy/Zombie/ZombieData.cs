using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "ZombieData", menuName = "ScriptableObjects/ZombieData")]
public class ZombieData : ScriptableObject
{
    public int hp = 100; // ������ ü��
    public int damage = 20; // ������ ���ݷ�
    public float moveSpeed = 2; // ������ �̵� �ӵ�
    public Color skinColor = Color.white; // ���� ����
}