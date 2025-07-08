using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>(); // 해당 씬에 있는 UIManager를 찾음
            }
            return _instance;
        }
    } // 싱글톤 인스턴스
    private static UIManager _instance; // 싱글톤 인스턴스 변수

    public Text ammoText; // 총알 수를 표시할 UI 텍스트
    public Text scoreText; // 점수를 표시할 UI 텍스트
    public Text WaveText; // 웨이브 정보를 표시할 UI 텍스트
    public GameObject gameOverUI; // 게임 오버 UI 패널

    public void UpdateAmmoTxt(int magAmmo, int remainAmmo)
    {
        this.ammoText.text = magAmmo + " / " + remainAmmo; // 총알 수를 UI에 업데이트
    }
    public void UpdateScoreTxt(int score)
    {
        this.scoreText.text = "Score" + score; // 점수를 UI에 업데이트
    }
    public void UpdateWaveTxt(int waves, int count)
    {
        this.WaveText.text = "Wave : " + waves + "\nEnemy Left : " + count; // 웨이브 정보를 UI에 업데이트
    }
    public void ShowGameOverUI(bool active)
    {
        gameOverUI.SetActive(active); // 게임 오버 UI 활성화
    }
    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 게임 씬을 다시 로드하여 게임 재시작
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
