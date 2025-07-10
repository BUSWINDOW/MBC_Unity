using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // 파일 입출력을 위한 네임 스페이스
using System.Runtime.Serialization.Formatters.Binary; // 실시간에서 직렬화 바이너리 파일 형식을 위한 네임 스페이스
using DataInfo;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    [SerializeField] private string dataPath; //파일이 저장될 물리적인 경로
    public void Initialize()
    {
        dataPath = Application.persistentDataPath + "/gameData.dat";
    }
    public void Save(GameData data)
    {
        BinaryFormatter bf = new BinaryFormatter(); // 직렬화 해주는애 선언
        FileStream file = File.Create(dataPath); // 파일 스트림 경로 잡아두기
        bf.Serialize(file, data); // 직렬화
        file.Close(); // 경로 다시 닫기
    }
    public GameData Load()
    {
        if (File.Exists(dataPath)) //만약 저장 파일이 존재한다면
        {
            BinaryFormatter bf =new BinaryFormatter();
            FileStream file = File.Open(dataPath, FileMode.Open);
            GameData data = (GameData)bf.Deserialize(file); // 직렬화 됐던걸 다시 역직렬화
            file.Close();
            return data;
        }
        return new GameData();
    }
}
