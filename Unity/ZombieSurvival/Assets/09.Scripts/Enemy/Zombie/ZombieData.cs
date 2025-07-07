using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "ZombieData", menuName = "ScriptableObjects/ZombieData")]
public class ZombieData : ScriptableObject
{
    public int hp; // ������ ü��
    public int damage; // ������ ���ݷ�
    public float moveSpeed; // ������ �̵� �ӵ�
    public Color skinColor; // ���� ����
}