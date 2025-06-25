using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isGameOver = false;
    public bool isPaused = false;

    public CanvasGroup inven_Open;
    public CanvasGroup inventory;

    public Button closeBtn;
    public Button openBtn;
    //public GameObject inven_Close;
    
    void Awake()
    {
        instance = this;
        
    }
    private void Start()
    {
        this.InventoryToggle(false);
        /*this.closeBtn.onClick.AddListener(() =>
        {
            Debug.Log("�ݱ� ��ư Ŭ��");
        });
        this.openBtn.onClick.AddListener(() => 
        {
            Debug.Log("���� ��ư Ŭ��");
        });*/
    }
    public void InventoryToggle(bool open)
    {
        Debug.Log(open ? "����" : "����");
        Cursor.visible = open;
        Time.timeScale = open ? 0 : 1;
        var player = GameObject.FindGameObjectWithTag("Player").GetComponents<MonoBehaviour>();
        foreach (var item in player) 
        {
            item.enabled = !open;
        }

        this.inventory.alpha = open ? 1 : 0;
        this.inventory.blocksRaycasts = open;
        this.inventory.interactable = open;
    }
}
