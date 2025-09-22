using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;
using TMPro.SpriteAssetUtilities;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    public Dictionary<Vector2Int, List<TrashInfo>> dicTrash = new Dictionary<Vector2Int, List<TrashInfo>>(); // 쓰레기 정보를 담을 딕셔너리

    public Dictionary<string, bool> dicPuzzle = new Dictionary<string, bool>(); // 퍼즐 클리어 여부를 담을 딕셔너리

    public bool IsLoadingFinish { get; private set; } = false; // 쓰레기 로딩이 끝났는지 여부
    public bool IsDataExist { get; private set; } = false; // 쓰레기 데이터가 존재하는지 여부
    public bool IsPDataExist { get; private set; } = false; // 쓰레기 데이터가 존재하는지 여부
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        if (File.Exists("./Assets/Resources/TrashMapData.json"))
        {
            this.IsDataExist = true; // 파일이 존재하면 데이터가 있다고 표시
            var json = File.ReadAllText("./Assets/Resources/TrashMapData.json"); // 파일이 존재하면 해당 파일을 읽어옴
            var data = JsonConvert.DeserializeObject<Dictionary<string, List<TrashInfo>>>(json); // JSON 데이터를 딕셔너리로 역직렬화
            this.dicTrash = data.ToDictionary(x => Vector2IntParse(x.Key), x => x.Value); // 딕셔너리로 변환
        }
        if (File.Exists("./Assets/Resources/PuzzleMapData.json"))
        {
            this.IsPDataExist = true; // 파일이 존재하면 데이터가 있다고 표시 // 테스트용이고 실제로는 DataExist쪽에 통합
            var json = File.ReadAllText("./Assets/Resources/PuzzleMapData.json"); // 파일이 존재하면 해당 파일을 읽어옴
            this.dicPuzzle = JsonConvert.DeserializeObject<Dictionary<string, bool>>(json); // JSON 데이터를 딕셔너리로 역직렬화
        }
        IsLoadingFinish = true; // 로딩 완료
    }
    private Vector2Int Vector2IntParse(string key)
    {
        var parts = key.Trim('(', ')') // 괄호 때고
            .Split(','); // 쉼표로 분리

        int x = int.Parse(parts[0]);
        int y = int.Parse(parts[1]);

        return new Vector2Int(x, y);
    }
    public async void SaveTrashData()
    {
        string json = null;
        string pJson = null;
        await Task.Run(() =>
        {
            json = JsonConvert.SerializeObject(dicTrash); // 딕셔너리를 JSON으로 직렬화
            pJson = JsonConvert.SerializeObject(dicPuzzle); // 딕셔너리를 JSON으로 직렬화
        });
        await File.WriteAllTextAsync("./Assets/Resources/TrashMapData.json", json);
        await File.WriteAllTextAsync("./Assets/Resources/PuzzleMapData.json", pJson);
        Debug.Log("저장됨");
    }
    private void OnApplicationQuit()
    {
        SaveTrashData(); //애플리케이션 종료 시 쓰레기 데이터를 저장
        // 후에 플레이어 위치, 플레이어가 소지중인 쓰레기 등도 저장해야한다.
    }
}
