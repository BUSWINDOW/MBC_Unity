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
    //주인공이 죽인 적 캐릭터 수
    [HideInInspector] public int KillCnt = 0;
    public Text killCntTxtUi;

    public DataManager dataManager;
    public GameData gameData;

    //인벤토리의 아이템이 변경되었을때 실행시킬 액션
    public static Action ItemChangeAction;

    //SlotList 게임 오브젝트를 저장할 변수
    public GameObject slotList;
    //itemList 하위에 있는 네개의 아이템을 저장할 배열
    public GameObject[] itemObjects;

    private void Awake()
    {
        Instance = this;
        this.dataManager = this.GetComponent<DataManager>();
        //this.dataManager = new DataManager();
        dataManager.Initialize();
        LoadGameData(); //세이브된 데이터 파일을 로드(불러오기)
    }
    
    void Start()
    {
        //OnInventory(false);
    }
    void LoadGameData()
    {
        //KillCnt = PlayerPrefs.GetInt("KillCnt", 0);//PlayerPrefs : 플레이어 Preferences
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
        //slot 하위에 있는 모든 slot을 추출
        var slots = slotList.GetComponentsInChildren<Transform>();
        //보유한 아이템 갯수만큼 반복
        for(int i = 0; i< gameData.equipItem.Count; i++)
        {
            //인벤토리 UI안에 있는 Slot 갯수만큼 반복
            for (int j = 1; j < slots.Length; j++)
            {
                if (slots[j].childCount > 0)
                {
                    continue; // 하위에 다른 아이템이 있으면 다음 인덱스로 넘어감
                }
                int itemIdx = (int)gameData.equipItem[i].type; // 보유 아이템 종류에 따라 인덱스 추출
                this.itemObjects[itemIdx].GetComponent<Transform>().SetParent(slots[j]);
                this.itemObjects[itemIdx].GetComponent<ItemInfo>().itemData = gameData.equipItem[i];
                break;
            }
        }
    }

    public void AddItem(Item item) // 인벤토리에 아이템 추가 시 게임 데이터 업데이트
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
    public void RemoveItem(Item item) // 인벤토리에 아이템 제거 시 게임 데이터 업데이트
    {
        if (!gameData.equipItem.Contains(item)) return;
        gameData.equipItem.Remove(item);//아이템 삭제

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
