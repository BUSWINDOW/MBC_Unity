using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public Button ExitBtn;
    public Button PlayBtn;
    void Start()
    {
        this.ExitBtn.onClick.AddListener(() =>
        {

#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
        this.PlayBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(1);
        });
    }

}
