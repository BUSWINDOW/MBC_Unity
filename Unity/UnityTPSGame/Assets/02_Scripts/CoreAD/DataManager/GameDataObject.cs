using System.Collections;
using System.Collections.Generic;
using DataInfo;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "GameDataSO" , menuName = "ScriptableObjects/GameDataSO" , order = 2)]
//Scriptable Object로 세이브 / 로드
public class GameDataObject :ScriptableObject
{
    public int killCnt = 0;
    public float hp = 120;
    public float damage = 25;
    public float speed = 6f;
    public List<Item> equipItem = new List<Item>();
}
