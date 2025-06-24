using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Collections.LowLevel.Unsafe; //씬매니저를 사용하기 위해 추가
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; 
    //싱글톤 패턴 ::객체생성은 한번만 되고 다른 클래스에서 쉽게 접근하기 위해
    //asteroid 프리팹 생성
    
    //asteroid 생성 주기 및 시간
    private float timePreve; //시간저장 
    [Header("bool GameOver")]//attribute 헤더 추가 
    public bool isGameOver = false; //게임오버 여부
    [Header("CameraShake Logic Member")]
    public bool isShake = false; //카메라 흔들림 여부
    public Vector3 PosCamera; //카메라 위치 저장
    public float beginTime;// 카메라가 흔들리기 시작한 시간
    [Header("HpBar UI")]
    public int hp; //체력깍이는 변수
    public int maxHp = 1000000;
    public Image hpBar; //체력바 UI
    public Text hpText; //체력 UI
    [Header("GameOver obj")]
    public GameObject gameOverObj; //게임오버 오브젝트 
    public Text scoreTxt;
    private float curScore = 0; //현재 점수
    private float plusScore = 1f; //점수 증가량


    [Header("운석 오브젝트 풀")]
    public GameObject asteroidPrefab;
    public GameObject asteroidPoolObject;
    public List<GameObject> asteroidPool = new List<GameObject>();
    public int maxAsteroidPoolCnt = 5;
    public int poolCnt = 0;

    public void CreatePool()
    {
        this.asteroidPoolObject = new GameObject("asteroid Pool");
        for (; poolCnt < this.maxAsteroidPoolCnt; )
        {
            CreateAsteroid();
        }
    }

    private void CreateAsteroid()
    {
        this.asteroidPool.Add(Instantiate(this.asteroidPrefab, this.asteroidPoolObject.transform));
        
        this.asteroidPool[this.poolCnt++].SetActive(false);
    }

    public GameObject GetAesteroid()
    {
        for(int i = 0; i < this.maxAsteroidPoolCnt; i++)
        {
            if (!this.asteroidPool[i].activeSelf)
            {
                return asteroidPool[i];
            }
        }
        CreateAsteroid();
        return asteroidPool[this.poolCnt-1];
    }

    void Start()
    {
        if(instance ==null)
         instance = this; //싱글톤 패턴
        else if(instance != this)
            Destroy(this.gameObject); //게임오브젝트 파괴
          
        
        DontDestroyOnLoad(this.gameObject); //씬이 바뀌어도 파괴되지 않음
        //게임시작전 현재시를 저장
        timePreve = Time.time;
        hp = maxHp; //체력 초기화

        this.CreatePool();

    }
    void Update()
    {
        if (isGameOver == true) //게임오버가 되면
        {
            return; //여기서 하위로 로직으로 진행 되지 않음
        }
        if (Time.time - timePreve > 2.5f && isGameOver == false)
        {

            AsteroidSpawn();
        }
        if (isShake == true) //만약에 카메라가 흔들린다면....
        {
            float x = Random.Range(-0.1f, 0.1f); //랜덤으로 x좌표 설정
            float y = Random.Range(-0.1f, 0.1f); //랜덤으로 y좌표 설정
            Camera.main.transform.position += new Vector3(x, y, 0f);
            if (Time.time - beginTime > 0.3f) //0.3초가 지나면 흔들림 종료
            {
                isShake = false; //카메라 흔들림 종료
                Camera.main.transform.position = PosCamera; //카메라 원래 위치로 복귀
            }
        }

        ScoreCount();

    }

    private void ScoreCount()
    {
        curScore += plusScore * Time.realtimeSinceStartup; //점수 증가
        //Time.realtimeSinceStartup : 게임이 시작한 이후의 시간을 초단위로 반환 readonly(읽기 전용) 속성
        scoreTxt.text = $" {Mathf.FloorToInt(curScore)}"; //점수 UI 갱신
    }

    public void TurnOn()
    {
        isShake = true; //카메라 흔들림 시작
        PosCamera = Camera.main.transform.position; //카메라가 흔들리기전 위치 저장
        beginTime = Time.time; //카메라 흔들리기 시작한 시간 저장
    }
    private void AsteroidSpawn()
    {
        //Debug.Log("스폰");
        float randomY = Random.Range(-4.0f, 4.0f); //랜덤으로 y좌표 설정
        //Instantiate(asteroidPrefab, new Vector3(12f, randomY, asteroidPrefab.transform.position.z), Quaternion.identity);
        var asteroid = this.GetAesteroid();
        asteroid.transform.position = new Vector3(12f, randomY, asteroid.transform.position.z);
        asteroid.SetActive(true);
        //오브젝트생성함수 (what?  , where, How 회전할 것인가 )
        timePreve = Time.time; //현재시간 저장
    }
    public void RoceketHealtPoint(int damage)
    {
        hp -= damage; //체력 감소
        hp = Mathf.Clamp(hp, 0, 100);
        hpBar.fillAmount = (float)hp / (float)maxHp; //체력바 UI 갱신
        hpText.text = $"HP : <color=#ff0000>{hp}</color>"; //체력 UI 갱신
        if (hp <= 0)
        {
           isGameOver = true; //게임오버
           gameOverObj.SetActive(true); //게임오버 오브젝트 활성화
            //게임오브젝트 를 활성화 비활성화 하는 함수 
            Invoke("LobbySceneMove", 3.0f);
            // 스트링 문자를 읽어서 원하는 시간에 호출 하는 함수 
        }
    }
    public void LobbySceneMove()
    {
        SceneManager.LoadScene("LobbyScene"); //씬을 LobbyScene으로  로딩
    }

}
