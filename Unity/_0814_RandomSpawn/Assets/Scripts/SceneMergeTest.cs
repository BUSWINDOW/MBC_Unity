using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMergeTest : MonoBehaviour
{
    public Button testBtn;
    private void Start()
    {
        testBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(1); // 내부 로직 관련
            SceneManager.LoadScene(2 , LoadSceneMode.Additive); // 스테이지
            SceneManager.LoadScene(4, LoadSceneMode.Additive); // 플레이어
        });
    }
}
