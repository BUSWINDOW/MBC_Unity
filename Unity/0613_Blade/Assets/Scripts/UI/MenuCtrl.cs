using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuCtrl : MonoBehaviour
{
    public RectTransform PauseBackGroundIMG;
    public RectTransform PauseMenu;
    public RectTransform SoundMenu;
    public RectTransform ScreenMenu;
    public GameObject player;
    public Toggle BGMToggle;
    public AudioSource BGMSource;
    public bool isPaused = false;
    void Start()
    {
        PauseBackGroundIMG = GameObject.Find("Canvas_UI").transform.GetChild(4).GetComponent<RectTransform>();
        PauseMenu = PauseBackGroundIMG.GetChild(0).GetComponent<RectTransform>();
        SoundMenu = PauseBackGroundIMG.GetChild(1).GetComponent<RectTransform>();
        ScreenMenu = PauseBackGroundIMG.GetChild(2).GetComponent<RectTransform>();
        this.BGMToggle = this.SoundMenu.GetChild(3).GetChild(2).GetComponent<Toggle>();

        this.BGMSource = Camera.main.GetComponent<AudioSource>();

        this.BGMToggle.onValueChanged.AddListener((isOn) => 
        {
            this.BGMSource.mute = isOn;
        });

        player = GameObject.FindWithTag("Player").gameObject;
    }
    void Update()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                {
                    ESCKeyDown();
                    break;
                }
            case RuntimePlatform.IPhonePlayer:
                {

                    break;
                }
            case RuntimePlatform.WindowsEditor:
                {
                    ESCKeyDown();
                    break;
                }
        }
    }

    private void ESCKeyDown()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape)) //esc∏¶ ¥≠∑∂¿ª∂ß
        {
            if (!isPaused) //∏ÿ√Á¡¯ ªÛ≈¬∞° æ∆¥œ∏È
                Pause(); //∏ÿ√„
            else//∏ÿ√Á¡¯ ªÛ≈¬∏È
            {
                Resume(); // ¿ÁΩ√¿€
            }
        }
    }

    private void Pause()
    {
        this.isPaused = !isPaused;
        Time.timeScale = 0;
        if (!this.PauseBackGroundIMG.gameObject.activeInHierarchy)
        {
            if (!PauseMenu.gameObject.activeInHierarchy)
            {
                PauseMenu.gameObject.SetActive(true);
                SoundMenu.gameObject.SetActive(false);
                ScreenMenu.gameObject.SetActive(false);
            }
            
        }
        this.PauseBackGroundIMG.gameObject.SetActive(true);

        
    }
    public void Resume()
    {
        Time.timeScale = 1;
        this.PauseBackGroundIMG.gameObject.SetActive(false);
        this.isPaused = !isPaused;
    }

    public void Sound(bool isOpen)
    {
        if (!isOpen)
        {
            PauseMenu.gameObject.SetActive(false);
            SoundMenu.gameObject.SetActive(true);
            ScreenMenu.gameObject.SetActive(false);
        }
        else
        {
            PauseMenu.gameObject.SetActive(true);
            SoundMenu.gameObject.SetActive(false);
            ScreenMenu.gameObject.SetActive(false);
        }
    }
    public void ScreenSetting(bool isOpen)
    {
        if (!isOpen)
        {
            PauseMenu.gameObject.SetActive(false);
            SoundMenu.gameObject.SetActive(false);
            ScreenMenu.gameObject.SetActive(true);
        }
        else
        {
            PauseMenu.gameObject.SetActive(true);
            SoundMenu.gameObject.SetActive(false);
            ScreenMenu.gameObject.SetActive(false);
        }
    }
}
