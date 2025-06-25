using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isGameOver = false;
    public GameObject[] panels;
    public GameObject panel_Pause;
    public GameObject Inventory;
    private readonly string player = "Player";
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
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

}
