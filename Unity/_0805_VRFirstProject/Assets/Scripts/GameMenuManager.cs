using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameMenuManager : MonoBehaviour
{
    public Transform head;
    public float spawnDist = 2;
    public GameObject gameMenu;
    public InputActionProperty showBtn;
    private void Update()
    {
        if (showBtn.action.triggered)
        {
            this.gameMenu.SetActive(!this.gameMenu.activeSelf);
            this.gameMenu.transform.position = this.head.position + this.head.forward * this.spawnDist;
            this.gameMenu.transform.LookAt(this.head.position);
            this.gameMenu.transform.forward *= -1; // UI를 다시 뒤집어놓기 위해
        }

        
    }
}
