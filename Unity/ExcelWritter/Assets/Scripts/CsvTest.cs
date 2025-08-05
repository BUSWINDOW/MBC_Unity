using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Windows;
using System;
using System.Text;
using System.IO;
using System.Reflection;
using UnityEngine.UIElements;
using System.Linq;
using System.Reflection.Emit;

public class CsvTest : MonoBehaviour {

    IEnumerator Start () 
	{
		Debug.Log("파일 생성중");
		if (System.IO.File.Exists("Assets/Resources/AwardData.csv"))
		{
			Debug.Log("이미 있음");
			//기존에 있던 내용은 내용 수정
			//기존에 없던 내용은 내용 추가
			// id 활용해서 판단
			
            // 1) StatData 타입 검색 (문자열 기반)
            var statDataType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name == "AwardData");

            // 2) 타입이 없으면 스킵
            if (statDataType == null)
            {
                Debug.LogWarning("StatData 클래스가 정의되지 않아 처리하지 않습니다.");
                yield break;
            }



            Dictionary<int, dynamic> statDatas = new Dictionary<int, dynamic>();
			var all = GameObject.FindObjectsOfType<Transform>(true);//모든 게임 오브젝트 다
			foreach (var go in all)
			{
				var scripts = go.gameObject.GetComponents<MonoBehaviour>();
				foreach (var script in scripts)
				{
					Type type = script.GetType();
					var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					//bool hasInt = false;
					foreach (FieldInfo field in fields)
					{
						
						if (field.FieldType == statDataType)
						{
							//StatData내용 받아오기
							var data = field.GetValue(script);
                            var idField = statDataType.GetField("id",
                    BindingFlags.Instance | BindingFlags.Public);
							int idValue = (int)idField.GetValue(data);


                            //Debug.Log(data.id);
                            statDatas.Add(idValue, data);
                        }
					}

				}
			}

			List<List<string>> modifiedTxt = new List<List<string>>();
			bool isChanged = false;

			using (var csv = new StreamReader("Assets/Resources/AwardData.csv"))
			{
				var txt = csv.ReadToEnd(); //내부 내용 다 읽어오는거 확인함
				var txtLine = txt.Split('\n'); // 줄 바꿈 기준으로 나눔
				for(int i = 0; i < 2; i++) //수정 버전에 초반부분 등록
				{
					var line = txtLine[i].Split(',');
                    line[line.Length - 1] = line[line.Length - 1].Replace("\r", "");
                    modifiedTxt.Add(line.ToList());
					/*for(int j = 0; j < line.Length; j++)
					{
                        modifiedTxt[i][j] = line[j];
                    }*/
				}
				for(int i = 2; i < txtLine.Length - 1; i++) // 첫번째건 이름, 두번째건 자료형
				{
					var id = int.Parse(txtLine[i].Split(',')[0]); // 각 줄을 ,로 나눠서 첫번째 내용 -> id를 int로
					if (statDatas.ContainsKey(id)) // 이미 있었다면
					{
						isChanged = true;
						var data = statDatas[id];
						List<string> line = new List<string>();
						//txtLine[i] = $"{data.id},{data.Name},{data.Hp},{data.Exp},{data.Str},{data.Dex},{data.Con},{data.Int}";

						line.Add(data.id.ToString());
						line.Add(data.Grade.ToString());
						line.Add(data.CompanyName.ToString());
						line.Add(data.AwardName.ToString());
						line.Add(data.Price.ToString());
						line.Add(data.ImagePath.ToString());



						/*line.Add(data.Name.ToString());
						line.Add(data.Level.ToString());
                        line.Add(data.Hp.ToString());
						line.Add(data.Exp.ToString());
						line.Add(data.Str.ToString());
						line.Add(data.Dex.ToString());
						line.Add(data.Con.ToString());
						line.Add(data.Int.ToString());*/
						
                        modifiedTxt.Add(line);
						statDatas.Remove(id);
                    }
					else
					{
                        var line = txtLine[i].Split(',');
                        line[line.Length - 1] = line[line.Length - 1].Replace("\r", "");
                        modifiedTxt.Add(line.ToList());
                    }
				}
			}
			if(statDatas.Count > 0)
			{
				isChanged = true;
				foreach (var Rdata in statDatas)
				{
                    List<string> line = new List<string>();
					//txtLine[i] = $"{data.id},{data.Name},{data.Hp},{data.Exp},{data.Str},{data.Dex},{data.Con},{data.Int}";
					var data = Rdata.Value;
					line.Add(data.id.ToString());
                    line.Add(data.Grade.ToString());
                    line.Add(data.CompanyName.ToString());
                    line.Add(data.AwardName.ToString());
                    line.Add(data.Price.ToString());
                    line.Add(data.ImagePath.ToString());


                    /*line.Add(data.Name.ToString());
                    line.Add(data.Level.ToString());
                    line.Add(data.Hp.ToString());
                    line.Add(data.Exp.ToString());
                    line.Add(data.Str.ToString());
                    line.Add(data.Dex.ToString());
                    line.Add(data.Con.ToString());
                    line.Add(data.Int.ToString());*/

                    modifiedTxt.Add(line);
                }
			}
			if (isChanged)
			{
                using (var writer = new CsvFileWriter("Assets/Resources/AwardData.csv"))
                {
                    //내용 추가하는 공간
                    foreach (var txt in modifiedTxt)
                    {
                        writer.WriteRow(txt);
                    }

                }
            }
			
        }
		else
		{
			using (var writer = new CsvFileWriter("Assets/Resources/AwardData.csv"))
			{
				List<string> columns = new List<string>() {"id", "Grade", "CompanyName", "AwardName", "Price", "ImagePath"};// making Index Row
				writer.WriteRow(columns);
				columns.Clear();



                columns.Add("int"); //id
                columns.Add("int"); // Grade
                columns.Add("string"); // CompanyName
                columns.Add("string"); // AwardName
                columns.Add("int"); // Price
                columns.Add("string"); // ImagePath

                /*columns.Add("int"); //id
				columns.Add("string"); // Name
				columns.Add("int"); // Level
				columns.Add("int"); // Hp
				columns.Add("int"); // Exp
				columns.Add("int"); // Str
				columns.Add("int"); // Dex
				columns.Add("int"); // Con
				columns.Add("int"); // Int*/
                writer.WriteRow(columns);
				columns.Clear();


				/*columns.Add("Bbulle"); // Name
				columns.Add("99"); // Level
				columns.Add("999"); // Hp
				columns.Add("5000"); // Exp
				columns.Add("99"); // Str
				columns.Add("50"); // Dex
				columns.Add("80"); // Con
				columns.Add("40"); // Int
				writer.WriteRow(columns);
				columns.Clear();

				columns.Add("Kukai"); // Name
				columns.Add("50"); // Level
				columns.Add("666"); // Hp
				columns.Add("3500"); // Exp
				columns.Add("66"); // Str
				columns.Add("66"); // Dex
				columns.Add("44"); // Con
				columns.Add("22"); // Int
				writer.WriteRow(columns);
				columns.Clear();*/
			}
		}
		yield return null;
		Debug.Log("파일 생성 완료");

    }
	public void ConvertCsvToXlsx(string csvFilePath, string xlsxFilePath)
	{

	}


}
