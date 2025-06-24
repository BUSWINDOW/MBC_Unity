using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{

    
    public void GameStart()
    {
      
        SceneManager.LoadScene("PlayScene"); //씬을 PlayScene으로  로딩
    }
    public void GameQuit()
    {
#if UNITY_EDITOR //매크로 지정 컴파일러가 에디터에서 실행중인지 확인 
        UnityEditor.EditorApplication.isPlaying = false; //에디터에서 게임 종료
#else
        Application.Quit(); //게임 종료
#endif

    }


}
