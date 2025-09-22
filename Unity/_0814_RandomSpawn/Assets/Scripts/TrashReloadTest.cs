using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrashReloadTest : MonoBehaviour
{
    public Button testBtn;
    public Button testBtn2;
    int loadIdx = 2;
    private void Start()
    {
        this.testBtn.onClick.AddListener(() =>
        {
            SceneManager.UnloadSceneAsync(this.loadIdx);// 기존 스테이지 언로드
            this.loadIdx = this.loadIdx == 2 ? 3 : 2;
            var sceneLoad = SceneManager.LoadSceneAsync(this.loadIdx, LoadSceneMode.Additive); // 새 스테이지 로드
            // 로드가 끝난 뒤 쓰레기 재생성

            // 제대로 다음 스테이지가 로드 되고 난 다음 실행되야함
            StartCoroutine(LoadSceneAfter(sceneLoad));
            
        });
        this.testBtn2.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });
    }
    IEnumerator LoadSceneAfter(AsyncOperation load)
    {
        while (!load.isDone)
        {
            yield return null;
        }
        GameObject.FindObjectOfType<TrashSpawnManager>().TrashRecreate();
        GameObject.FindObjectOfType<MapManager>().PuzzleReset();
        
    }
}
