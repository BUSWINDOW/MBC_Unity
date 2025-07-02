using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public Button playBtn;
    public Button quitBtn;
    void Awake()
    {
        playBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("FadeInScene");
            //SceneManager.LoadScene("SampleScene", LoadSceneMode.Additive);
        });
        quitBtn.onClick.AddListener(() => 
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
    }
}
