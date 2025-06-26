using System;
using System.Collections;
using System.Collections.Generic;
using DataInfo;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isGameOver = false;
    public GameObject[] panels;
    public GameObject panel_Pause;
    public GameObject Inventory;
    private readonly string player = "Player";
    //���ΰ��� ���� �� ĳ���� ��
    [HideInInspector] public int KillCnt = 0;
    public Text killCntTxtUi;

    public DataManager dataManager;
    public GameData gameData;

    //�κ��丮�� �������� ����Ǿ����� �����ų �׼�
    public static Action ItemChangeAction;

    //SlotList ���� ������Ʈ�� ������ ����
    public GameObject slotList;
    //itemList ������ �ִ� �װ��� �������� ������ �迭
    public GameObject[] itemObjects;

    private void Awake()
    {
        Instance = this;
        this.dataManager = this.GetComponent<DataManager>();
        //this.dataManager = new DataManager();
        dataManager.Initialize();
        LoadGameData(); //���̺�� ������ ������ �ε�(�ҷ�����)
    }
    
    void Start()
    {
        //OnInventory(false);
    }
    void LoadGameData()
    {
        //KillCnt = PlayerPrefs.GetInt("KillCnt", 0);//PlayerPrefs : �÷��̾� Preferences
        gameData = this.dataManager.Load();

        if(gameData.equipItem.Count > 0)
        {
            this.InventorySetUp();
        }
        this.killCntTxtUi.text = $"Kill : <color=#ff0000>{this.gameData.killCnt.ToString("000")}</color>";
    }
    private void SaveGameData()
    {
        this.dataManager.Save(this.gameData);
    }
    void InventorySetUp()
    {
        //slot ������ �ִ� ��� slot�� ����
        var slots = slotList.GetComponentsInChildren<Transform>();
        //������ ������ ������ŭ �ݺ�
        for(int i = 0; i< gameData.equipItem.Count; i++)
        {
            //�κ��丮 UI�ȿ� �ִ� Slot ������ŭ �ݺ�
            for (int j = 1; j < slots.Length; j++)
            {
                if (slots[j].childCount > 0)
                {
                    continue; // ������ �ٸ� �������� ������ ���� �ε����� �Ѿ
                }
                int itemIdx = (int)gameData.equipItem[i].type; // ���� ������ ������ ���� �ε��� ����
                this.itemObjects[itemIdx].GetComponent<Transform>().SetParent(slots[j]);
                this.itemObjects[itemIdx].GetComponent<ItemInfo>().itemData = gameData.equipItem[i];
                break;
            }
        }
    }

    public void AddItem(Item item) // �κ��丮�� ������ �߰� �� ���� ������ ������Ʈ
    {
        if (gameData.equipItem.Contains(item)) return;
        gameData.equipItem.Add(item);

        switch (item.type)
        {
            case Item.ItemType.HP:
                {
                    if(item.calc == Item.ItemCalc.Value)
                    {
                        gameData.hp += item.value;
                    }
                    else
                        gameData.hp += gameData.hp * item.value;
                        break;
                }
            case Item.ItemType.Speed:
                {
                    if (item.calc == Item.ItemCalc.Value)
                    {
                        gameData.speed += item.value;
                    }
                    else
                        gameData.speed += gameData.speed * item.value;
                    break;
                }
            case Item.ItemType.Shock:
                {
                    if (item.calc == Item.ItemCalc.Value)
                    {
                        gameData.damage += item.value;
                    }
                    else
                        gameData.damage += gameData.damage * item.value;
                    break;
                }
            case Item.ItemType.Granade:
                {

                    break;
                }
        }
        ItemChangeAction();
    }
    public void RemoveItem(Item item) // �κ��丮�� ������ ���� �� ���� ������ ������Ʈ
    {
        if (!gameData.equipItem.Contains(item)) return;
        gameData.equipItem.Remove(item);//������ ����

        switch (item.type)
        {
            case Item.ItemType.HP:
                {
                    if (item.calc == Item.ItemCalc.Value)
                    {
                        gameData.hp -= item.value;
                    }
                    else
                        gameData.hp = gameData.hp / (1.0f + item.value);
                    break;
                }
            case Item.ItemType.Speed:
                {
                    if (item.calc == Item.ItemCalc.Value)
                    {
                        gameData.speed -= item.value;
                    }
                    else
                        gameData.speed = gameData.speed / (1.0f+ item.value);
                    break;
                }
            case Item.ItemType.Shock:
                {
                    if (item.calc == Item.ItemCalc.Value)
                    {
                        gameData.damage -= item.value;
                    }
                    else
                        gameData.damage = gameData.damage / (1.0f+ item.value);
                    break;
                }
            case Item.ItemType.Granade:
                {

                    break;
                }
        }

        ItemChangeAction();
    }

    private bool isPaused;
    public void OnPause()
    {
        StopGame();
        
        
    }

    private void StopGame()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;

        var playerObj = GameObject.FindGameObjectWithTag(player);
        var scripts = playerObj.GetComponents<MonoBehaviour>();
        foreach (var item in scripts)
        {
            item.enabled = !isPaused;
        }
        foreach (var item in panels)
        {
            var canvasGroup = item.GetComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = !isPaused;
        }
    }

    public void OnInventory(bool isOpened)
    {
        StopGame();
        var cvGroup = Inventory.GetComponent<CanvasGroup>();
        cvGroup.interactable = isOpened;
        cvGroup.blocksRaycasts = isOpened;
        cvGroup.alpha = isOpened ? 1 : 0;
        var canvasGroup = this.panel_Pause.GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = !isOpened;
    }
    public void IncKillCnt()
    {
        ++this.gameData.killCnt;
        //PlayerPrefs.SetInt("KillCnt", this.KillCnt);
        this.killCntTxtUi.text = $"Kill : <color=#ff0000>{this.gameData.killCnt.ToString("000")}</color>";
    }
    /*private void OnDisable()
    {
        this.dataManager.Save(this.gameData);
    }*/
    private void OnApplicationQuit()
    {
        SaveGameData();
    }


}
