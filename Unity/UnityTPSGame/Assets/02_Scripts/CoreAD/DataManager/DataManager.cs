using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // ���� ������� ���� ���� �����̽�
using System.Runtime.Serialization.Formatters.Binary; // �ǽð����� ����ȭ ���̳ʸ� ���� ������ ���� ���� �����̽�
using DataInfo;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    [SerializeField] private string dataPath; //������ ����� �������� ���
    public void Initialize()
    {
        dataPath = Application.persistentDataPath + "/gameData.dat";
    }
    public void Save(GameData data)
    {
        BinaryFormatter bf = new BinaryFormatter(); // ����ȭ ���ִ¾� ����
        FileStream file = File.Create(dataPath); // ���� ��Ʈ�� ��� ��Ƶα�
        bf.Serialize(file, data); // ����ȭ
        file.Close(); // ��� �ٽ� �ݱ�
    }
    public GameData Load()
    {
        if (File.Exists(dataPath)) //���� ���� ������ �����Ѵٸ�
        {
            BinaryFormatter bf =new BinaryFormatter();
            FileStream file = File.Open(dataPath, FileMode.Open);
            GameData data = (GameData)bf.Deserialize(file); // ����ȭ �ƴ��� �ٽ� ������ȭ
            file.Close();
            return data;
        }
        return new GameData();
    }
}
