using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

public class DataManager : MonoBehaviour
{
    JsonConverter json;
    public Dictionary<int, AwardData> awardDic;
    public static DataManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        TextAsset asset = Resources.Load<TextAsset>("AwardData/AwardData");
        var datas = JsonConvert.DeserializeObject<AwardData[]>(asset.text);
        this.awardDic = datas.ToDictionary(x => x.id);
    }
}
