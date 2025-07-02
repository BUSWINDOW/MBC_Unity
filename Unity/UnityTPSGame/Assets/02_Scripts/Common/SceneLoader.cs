using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public CanvasGroup FadeCG;
    [Range(0.5f, 2)] public float fadeDuration = 1.0f;
    public Dictionary<string , LoadSceneMode> loadScenes = new Dictionary<string , LoadSceneMode>();

    //호출할 씬의 정보 설정
    void InitSceneInfo()
    {
        this.loadScenes.Add("LevelScene", LoadSceneMode.Additive);
        this.loadScenes.Add("SampleScene" , LoadSceneMode.Additive);
    }
    private IEnumerator Start()
    { 
        this.InitSceneInfo();

        this.FadeCG.alpha = 1.0f;

        foreach(var a in loadScenes)
        {
            yield return StartCoroutine(LoadScene(a.Key, a.Value)); //각 씬 정보에 대해서 코루틴을 돌림
        }
        this.Fade(0);
    }

    private IEnumerator LoadScene(string key, LoadSceneMode value)
    {
        yield return SceneManager.LoadSceneAsync(key, value); //코루틴이 돌아갈때마다 비동기 방식으로 씬을 로드시키고
                                                              //로드가 완료 될때까지 대기

        //호출된 씬 활성화
        Scene LoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1); 
        SceneManager.SetActiveScene(LoadedScene);
    }
    void Fade(float finalAlpha)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("LevelScene")); // 라이트맵이 깨지는걸 방지하기위해
                                                                                // 스테이지 씬을 활성화

        FadeCG.blocksRaycasts = true;
        this.FadeCG.DOFade(finalAlpha, this.fadeDuration).OnComplete(() =>
        {
            this.FadeCG.blocksRaycasts = false;
        }).OnComplete(() =>
        {
            SceneManager.UnloadSceneAsync("FadeInScene");
        });
        
    }
}
