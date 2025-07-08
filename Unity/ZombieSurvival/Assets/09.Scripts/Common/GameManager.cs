using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>(); // �ش� ���� �ִ� UIManager�� ã��
            }
            return _instance;
        }
    } // �̱��� �ν��Ͻ�
    private static GameManager _instance; // �̱��� �ν��Ͻ� ����
    public bool IsGameOver { get; private set; }
    private int score = 0; // ���� ����
    private void Awake()
    {
        if(Instance != this)
        {
            Destroy(this.gameObject); // �̱��� �ν��Ͻ��� �̹� �����ϸ� ���� ������Ʈ�� �ı�
        }
    }
    void Start()
    {
        FindObjectOfType<WomanHealth>().DieAction += this.OnGameOver; // �÷��̾ �׾��� �� ���� ���� ó��
    }

    private void OnGameOver()
    {
        this.IsGameOver = true; // ���� ���� ���·� ����
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowGameOverUI(true); // ���� ���� �г� ǥ��
        }
    }
    public void AddScore(int score)
    {
        if(this.IsGameOver) return; // ���� ���� ���¸� ���� �߰����� ����
        this.score += score; // ���� �߰�
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateScoreTxt(this.score); // UI ������Ʈ
        }
    }

    void Update()
    {
        
    }
}
