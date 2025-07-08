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
                _instance = FindObjectOfType<GameManager>(); // 해당 씬에 있는 UIManager를 찾음
            }
            return _instance;
        }
    } // 싱글톤 인스턴스
    private static GameManager _instance; // 싱글톤 인스턴스 변수
    public bool IsGameOver { get; private set; }
    private int score = 0; // 게임 점수
    private void Awake()
    {
        if(Instance != this)
        {
            Destroy(this.gameObject); // 싱글톤 인스턴스가 이미 존재하면 현재 오브젝트를 파괴
        }
    }
    void Start()
    {
        FindObjectOfType<WomanHealth>().DieAction += this.OnGameOver; // 플레이어가 죽었을 때 게임 오버 처리
    }

    private void OnGameOver()
    {
        this.IsGameOver = true; // 게임 오버 상태로 변경
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowGameOverUI(true); // 게임 오버 패널 표시
        }
    }
    public void AddScore(int score)
    {
        if(this.IsGameOver) return; // 게임 오버 상태면 점수 추가하지 않음
        this.score += score; // 점수 추가
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateScoreTxt(this.score); // UI 업데이트
        }
    }

    void Update()
    {
        
    }
}
