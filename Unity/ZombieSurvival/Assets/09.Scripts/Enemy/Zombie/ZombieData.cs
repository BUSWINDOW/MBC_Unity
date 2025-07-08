using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "ZombieData", menuName = "ScriptableObjects/ZombieData")]
public class ZombieData : ScriptableObject
{
    public int hp = 100; // 좀비의 체력
    public int damage = 20; // 좀비의 공격력
    public float moveSpeed = 2; // 좀비의 이동 속도
    public Color skinColor = Color.white; // 좀비 색상
}