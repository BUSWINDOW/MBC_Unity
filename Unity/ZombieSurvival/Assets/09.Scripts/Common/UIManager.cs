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
                _instance = FindObjectOfType<UIManager>(); // �ش� ���� �ִ� UIManager�� ã��
            }
            return _instance;
        }
    } // �̱��� �ν��Ͻ�
    private static UIManager _instance; // �̱��� �ν��Ͻ� ����

    public Text ammoText; // �Ѿ� ���� ǥ���� UI �ؽ�Ʈ
    public Text scoreText; // ������ ǥ���� UI �ؽ�Ʈ
    public Text WaveText; // ���̺� ������ ǥ���� UI �ؽ�Ʈ
    public GameObject gameOverUI; // ���� ���� UI �г�

    public void UpdateAmmoTxt(int magAmmo, int remainAmmo)
    {
        this.ammoText.text = magAmmo + " / " + remainAmmo; // �Ѿ� ���� UI�� ������Ʈ
    }
    public void UpdateScoreTxt(int score)
    {
        this.scoreText.text = "Score" + score; // ������ UI�� ������Ʈ
    }
    public void UpdateWaveTxt(int waves, int count)
    {
        this.WaveText.text = "Wave : " + waves + "\nEnemy Left : " + count; // ���̺� ������ UI�� ������Ʈ
    }
    public void ShowGameOverUI(bool active)
    {
        gameOverUI.SetActive(active); // ���� ���� UI Ȱ��ȭ
    }
    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ���� ���� �ٽ� �ε��Ͽ� ���� �����
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
