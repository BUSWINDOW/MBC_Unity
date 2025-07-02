using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Loading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneLoader : MonoBehaviour
{
    public Button startBtn;
    public CanvasGroup dim;
    public Image BG;
    [Range(0.5f,2)] public float duration;
    Dictionary<string, LoadSceneMode> loadScenes = new Dictionary<string, LoadSceneMode>();
    void Start()
    {
        Init();
        this.startBtn.onClick.AddListener(() =>
        {
            this.dim.blocksRaycasts = true;
            this.dim.DOFade(1, duration).OnComplete(() =>
            {
                this.BG.gameObject.SetActive(false);
                this.startBtn.gameObject.SetActive(false);
                foreach(var scene in this.loadScenes)
                {
                    StartCoroutine(SceneLoading(scene.Key, scene.Value));
                }
                this.dim.blocksRaycasts = false;
                this.dim.DOFade(0, duration).OnComplete(() => 
                {
                    SceneManager.UnloadSceneAsync("StartScene");
                });
            });
        });
    }

    private IEnumerator SceneLoading(string key, LoadSceneMode value)
    {
        yield return SceneManager.LoadSceneAsync(key, value);
    }

    void Init()
    {
        this.loadScenes.Add("StageScene", LoadSceneMode.Additive);
        this.loadScenes.Add("MainScene", LoadSceneMode.Additive);
    }
}
