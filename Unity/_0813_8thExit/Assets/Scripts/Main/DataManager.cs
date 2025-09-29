using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;
using DG.Tweening.Plugins.Core.PathCore;
using Unity.VisualScripting.FullSerializer;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    public Dictionary<int,bool> dicAnomalySeen;
    public bool IsDataExist { get; private set; }
    public bool IsLoadingFinish { get; private set; } = false;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject); // 씬이 바뀌어도 파괴되지 않도록 설정
        LoadData();
    }

    private void LoadData()
    {
        if (File.Exists("./Assets/Resources/anomalyData.json"))
        {
            this.IsDataExist = true;
            var anomalyJson = File.ReadAllText("./Assets/Resources/anomalyData.json"); // 파일이 존재하면 해당 파일을 읽어옴
            this.dicAnomalySeen = JsonConvert.DeserializeObject<Dictionary<int,bool>>(anomalyJson);
        }
        else
        {
            this.IsDataExist = false;
            this.dicAnomalySeen = new Dictionary<int, bool>();
        }
        this.IsLoadingFinish = true;
    }


    public void SaveData()
    {
        string anomalyJson = null;
        anomalyJson = JsonConvert.SerializeObject(this.dicAnomalySeen);
        File.WriteAllText("./Assets/Resources/anomalyData.json", anomalyJson);
    }
    private void OnApplicationQuit()
    {
        this.SaveData();
    }
}
