using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace CsvConverter
{
    public class ReadCsv
    {
        public static string[] name;
        public static string[] type;
        public static string[] tests;
        public static List<List<string>> inside = new List<List<string>>();
        List<string> list;
        public ReadCsv(string filename)
        {
            string test = File.ReadAllText("./" + filename + ".csv");
            //string test = File.ReadAllText("./item_data.xlsx");
            //Console.WriteLine(test);
            tests = test.Split('\n');
            name = tests[0].Split(',');
            name[name.Length - 1] = name[name.Length - 1].Replace("\r", "");
            type = tests[1].Split(',');
            type[type.Length - 1] = type[type.Length - 1].Replace("\r", "");


            for (int i = 2; i < tests.Length-1; i++) //생성된 csv파일은 한줄이 추가로 있기에 그 줄 제외
            {
                string[] temp = tests[i].Split(',');

                list = new List<string>();
                foreach (string a in temp)
                {
                    list.Add(a);
                }
                list[list.Count - 1] = list[list.Count - 1].Replace("\r", "");

                inside.Add(list);
            }
            string json = "";
            json = json + "[\n";
            for (int i = 0; i < inside.Count; i++)
            {
                json = json + "  {\n";
                for (int j = 0; j < inside[i].Count; j++)
                {
                    //이 부분 수정 필요, string의 경우에만 따옴표가 붙고, 나머진 안붙어야함
                    // 이 값이 string인지 CS만드는 쪽과 연계해서 확인 필요
                    json = json + string.Format($"    \"{name[j]}\": \"{inside[i][j]}\",\n");
                }
                json = json.Substring(0, json.Length - 2);
                json = json + "\n  },\n";

            }
            json = json.Substring(0, json.Length - 2);
            json = json + "\n]";
            File.WriteAllText("./" + filename + ".json", json);
        }
    }
}