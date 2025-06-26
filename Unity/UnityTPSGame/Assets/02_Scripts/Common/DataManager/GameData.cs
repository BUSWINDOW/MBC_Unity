using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DataInfo
{
    [System.Serializable]
    public class GameData
    {
        public int killCnt = 0;
        public float hp = 120f;
        public float damage = 25;
        public float speed = 300;

        public List<Item> equipItem = new List<Item>();
    }
    [System.Serializable]
    public class Item
    {
        public enum ItemType
        {
            HP, Speed, Granade, Shock
        }
        public enum ItemCalc
        {
            Value, Persent
        }
        public ItemType type;
        public ItemCalc calc;
        public string name;
        public string desc;
        public float value;
    }
}