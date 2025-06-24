using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Collections.LowLevel.Unsafe; //���Ŵ����� ����ϱ� ���� �߰�
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; 
    //�̱��� ���� ::��ü������ �ѹ��� �ǰ� �ٸ� Ŭ�������� ���� �����ϱ� ����
    //asteroid ������ ����
    
    //asteroid ���� �ֱ� �� �ð�
    private float timePreve; //�ð����� 
    [Header("bool GameOver")]//attribute ��� �߰� 
    public bool isGameOver = false; //���ӿ��� ����
    [Header("CameraShake Logic Member")]
    public bool isShake = false; //ī�޶� ��鸲 ����
    public Vector3 PosCamera; //ī�޶� ��ġ ����
    public float beginTime;// ī�޶� ��鸮�� ������ �ð�
    [Header("HpBar UI")]
    public int hp; //ü�±��̴� ����
    public int maxHp = 1000000;
    public Image hpBar; //ü�¹� UI
    public Text hpText; //ü�� UI
    [Header("GameOver obj")]
    public GameObject gameOverObj; //���ӿ��� ������Ʈ 
    public Text scoreTxt;
    private float curScore = 0; //���� ����
    private float plusScore = 1f; //���� ������


    [Header("� ������Ʈ Ǯ")]
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
         instance = this; //�̱��� ����
        else if(instance != this)
            Destroy(this.gameObject); //���ӿ�����Ʈ �ı�
          
        
        DontDestroyOnLoad(this.gameObject); //���� �ٲ� �ı����� ����
        //���ӽ����� ����ø� ����
        timePreve = Time.time;
        hp = maxHp; //ü�� �ʱ�ȭ

        this.CreatePool();

    }
    void Update()
    {
        if (isGameOver == true) //���ӿ����� �Ǹ�
        {
            return; //���⼭ ������ �������� ���� ���� ����
        }
        if (Time.time - timePreve > 2.5f && isGameOver == false)
        {

            AsteroidSpawn();
        }
        if (isShake == true) //���࿡ ī�޶� ��鸰�ٸ�....
        {
            float x = Random.Range(-0.1f, 0.1f); //�������� x��ǥ ����
            float y = Random.Range(-0.1f, 0.1f); //�������� y��ǥ ����
            Camera.main.transform.position += new Vector3(x, y, 0f);
            if (Time.time - beginTime > 0.3f) //0.3�ʰ� ������ ��鸲 ����
            {
                isShake = false; //ī�޶� ��鸲 ����
                Camera.main.transform.position = PosCamera; //ī�޶� ���� ��ġ�� ����
            }
        }

        ScoreCount();

    }

    private void ScoreCount()
    {
        curScore += plusScore * Time.realtimeSinceStartup; //���� ����
        //Time.realtimeSinceStartup : ������ ������ ������ �ð��� �ʴ����� ��ȯ readonly(�б� ����) �Ӽ�
        scoreTxt.text = $" {Mathf.FloorToInt(curScore)}"; //���� UI ����
    }

    public void TurnOn()
    {
        isShake = true; //ī�޶� ��鸲 ����
        PosCamera = Camera.main.transform.position; //ī�޶� ��鸮���� ��ġ ����
        beginTime = Time.time; //ī�޶� ��鸮�� ������ �ð� ����
    }
    private void AsteroidSpawn()
    {
        //Debug.Log("����");
        float randomY = Random.Range(-4.0f, 4.0f); //�������� y��ǥ ����
        //Instantiate(asteroidPrefab, new Vector3(12f, randomY, asteroidPrefab.transform.position.z), Quaternion.identity);
        var asteroid = this.GetAesteroid();
        asteroid.transform.position = new Vector3(12f, randomY, asteroid.transform.position.z);
        asteroid.SetActive(true);
        //������Ʈ�����Լ� (what?  , where, How ȸ���� ���ΰ� )
        timePreve = Time.time; //����ð� ����
    }
    public void RoceketHealtPoint(int damage)
    {
        hp -= damage; //ü�� ����
        hp = Mathf.Clamp(hp, 0, 100);
        hpBar.fillAmount = (float)hp / (float)maxHp; //ü�¹� UI ����
        hpText.text = $"HP : <color=#ff0000>{hp}</color>"; //ü�� UI ����
        if (hp <= 0)
        {
           isGameOver = true; //���ӿ���
           gameOverObj.SetActive(true); //���ӿ��� ������Ʈ Ȱ��ȭ
            //���ӿ�����Ʈ �� Ȱ��ȭ ��Ȱ��ȭ �ϴ� �Լ� 
            Invoke("LobbySceneMove", 3.0f);
            // ��Ʈ�� ���ڸ� �о ���ϴ� �ð��� ȣ�� �ϴ� �Լ� 
        }
    }
    public void LobbySceneMove()
    {
        SceneManager.LoadScene("LobbyScene"); //���� LobbyScene����  �ε�
    }

}
