using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // ���� ������� ���� ���� �����̽�
using System.Runtime.Serialization.Formatters.Binary;
using System; // �ǽð����� ����ȭ ���̳ʸ� ���� ������ ���� ���� �����̽�

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    [SerializeField] private string dataPath; //������ ����� �������� ���
    public GameData gameData;
    public Action ItemApplyAction;

    //SlotList ���� ������Ʈ�� ������ ����
    public GameObject slotList;
    //itemList ������ �ִ� �װ��� �������� ������ �迭
    public GameObject[] itemObjects;

    void Awake()
    {
        instance = this;
        dataPath = Application.persistentDataPath + "/gameData.dat";
    }
    public void Save(GameData data)
    {
        BinaryFormatter bf = new BinaryFormatter(); // ����ȭ ���ִ¾� ����
        FileStream file = File.Create(dataPath); // ���� ��Ʈ�� ��� ��Ƶα�
        bf.Serialize(file, data); // ����ȭ
        file.Close(); // ��� �ٽ� �ݱ�
    }
    public void Load()
    {
        if (File.Exists(dataPath)) //���� ���� ������ �����Ѵٸ�
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(dataPath, FileMode.Open);
            this.gameData = (GameData)bf.Deserialize(file); // ����ȭ �ƴ��� �ٽ� ������ȭ
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
        //slot ������ �ִ� ��� slot�� ����
        var slots = slotList.GetComponentsInChildren<Transform>();
        //������ ������ ������ŭ �ݺ�
        for (int i = 0; i < gameData.items.Count; i++)
        {
            //�κ��丮 UI�ȿ� �ִ� Slot ������ŭ �ݺ�
            for (int j = 1; j < slots.Length; j++)
            {
                if (slots[j].childCount > 0)
                {
                    continue; // ������ �ٸ� �������� ������ ���� �ε����� �Ѿ
                }
                int itemIdx = (int)gameData.items[i].Idx; // ���� ������ ������ ���� �ε��� ����
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
