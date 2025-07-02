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

    //ȣ���� ���� ���� ����
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
            yield return StartCoroutine(LoadScene(a.Key, a.Value)); //�� �� ������ ���ؼ� �ڷ�ƾ�� ����
        }
        this.Fade(0);
    }

    private IEnumerator LoadScene(string key, LoadSceneMode value)
    {
        yield return SceneManager.LoadSceneAsync(key, value); //�ڷ�ƾ�� ���ư������� �񵿱� ������� ���� �ε��Ű��
                                                              //�ε尡 �Ϸ� �ɶ����� ���

        //ȣ��� �� Ȱ��ȭ
        Scene LoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1); 
        SceneManager.SetActiveScene(LoadedScene);
    }
    void Fade(float finalAlpha)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("LevelScene")); // ����Ʈ���� �����°� �����ϱ�����
                                                                                // �������� ���� Ȱ��ȭ

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
