using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // 파일 입출력을 위한 네임 스페이스
using System.Runtime.Serialization.Formatters.Binary;
using System; // 실시간에서 직렬화 바이너리 파일 형식을 위한 네임 스페이스

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    [SerializeField] private string dataPath; //파일이 저장될 물리적인 경로
    public GameData gameData;
    public Action ItemApplyAction;

    //SlotList 게임 오브젝트를 저장할 변수
    public GameObject slotList;
    //itemList 하위에 있는 네개의 아이템을 저장할 배열
    public GameObject[] itemObjects;

    void Awake()
    {
        instance = this;
        dataPath = Application.persistentDataPath + "/gameData.dat";
    }
    public void Save(GameData data)
    {
        BinaryFormatter bf = new BinaryFormatter(); // 직렬화 해주는애 선언
        FileStream file = File.Create(dataPath); // 파일 스트림 경로 잡아두기
        bf.Serialize(file, data); // 직렬화
        file.Close(); // 경로 다시 닫기
    }
    public void Load()
    {
        if (File.Exists(dataPath)) //만약 저장 파일이 존재한다면
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(dataPath, FileMode.Open);
            this.gameData = (GameData)bf.Deserialize(file); // 직렬화 됐던걸 다시 역직렬화
            file.Close();
            
        }
        else
        {
            this.gameData = new GameData();
        }
        if(this.gameData.items.Count > 0)
        {
            this.InventorySetUp();
        }
        //return this.gameData;
    }

    void InventorySetUp()
    {
        //slot 하위에 있는 모든 slot을 추출
        var slots = slotList.GetComponentsInChildren<Transform>();
        //보유한 아이템 갯수만큼 반복
        for (int i = 0; i < gameData.items.Count; i++)
        {
            //인벤토리 UI안에 있는 Slot 갯수만큼 반복
            for (int j = 1; j < slots.Length; j++)
            {
                if (slots[j].childCount > 0)
                {
                    continue; // 하위에 다른 아이템이 있으면 다음 인덱스로 넘어감
                }
                int itemIdx = (int)gameData.items[i].Idx; // 보유 아이템 종류에 따라 인덱스 추출
                this.itemObjects[itemIdx].GetComponent<Transform>().SetParent(slots[j]);
                this.itemObjects[itemIdx].GetComponent<ItemInfo>().data = gameData.items[i];
                break;
            }
        }
    }

    public void AddItem(ItemData item)
    {
        if (this.gameData.items.Contains(item)) return;
        this.gameData.items.Add(item);

        switch (item.Idx)
        {
            case ItemData.eItemIdx.Hp:
                {
                    this.gameData.hp += item.Value;
                    break;
                }
            case ItemData.eItemIdx.Speed:
                {
                    this.gameData.speed += item.Value;
                    break;
                }
            case ItemData.eItemIdx.Shock:
                {
                    this.gameData.damage += item.Value;
                    break;
                }
            case ItemData.eItemIdx.Granade:
                {
                    this.gameData.granade += item.Value;
                    break;
                }
        }
        this.ItemApplyAction();
    }
    public void RemoveItem(ItemData item)
    {
        if (!this.gameData.items.Contains(item)) return;
        this.gameData.items.Remove(item);

        switch (item.Idx)
        {
            case ItemData.eItemIdx.Hp:
                {
                    this.gameData.hp -= item.Value;
                    break;
                }
            case ItemData.eItemIdx.Speed:
                {
                    this.gameData.speed -= item.Value;
                    break;
                }
            case ItemData.eItemIdx.Shock:
                {
                    this.gameData.damage -= item.Value;
                    break;
                }
            case ItemData.eItemIdx.Granade:
                {
                    this.gameData.granade -= item.Value;
                    break;
                }
        }
        this.ItemApplyAction();
    }
    private void OnApplicationQuit()
    {
        this.Save(this.gameData);
    }
}
