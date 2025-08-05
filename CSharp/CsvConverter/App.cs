using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CsvConverter
{
    public class App
    {
        
        public App()
        {
            Console.WriteLine(".csv를 빼고 파일이름을 입력하세요.");

            string name = Console.ReadLine();
            if (!File.Exists("./" + name + ".csv")) { 
                Console.WriteLine("파일이 존재하지 않습니다.");
                Console.ReadLine();
                return; }

            ReadCsv test = new ReadCsv(name); // 이 줄에서 json은 생성됨

            Console.WriteLine("C# 스크립트가 필요하십니까?(Y/N)");
            var input = Console.ReadLine();
            if (input == "Y" || input == "y")
            {
                CreateCS sample = new CreateCS(name);

                for (int i = 0; i < ReadCsv.name.Length; i++)
                {
                    sample.AddFields(ReadCsv.type[i], ReadCsv.name[i]);
                }
                sample.GenerateCSharpCode(name + ".cs");
            }
            else
            {
                return;
            }
        }
    }
}