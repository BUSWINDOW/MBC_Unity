using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace _0617_CSharp
{
    class MyClass2
    {
        [Obsolete("이 메서드는 폐기 되었습니다. NewMethod사용 바람")]
        public void OldMethod()
        {
            Console.WriteLine("old");
        }
        public void NewMethod()
        {
            Console.WriteLine("new");
        }
    }
    public static class Trace
    {
        public static void WriteLine(string message , [CallerFilePath]string file = "", [CallerLineNumber]int line = 0, [CallerMemberName]string member = "")
        {
            Console.Write($"{file}(Line : {line} {member} {message})");
        }
    }
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple =  true)]
    class History : System.Attribute
    {
        private string programmer;
        public double Version {  get; set; }
        public string Changes {  get; set; }
        public History(string programmer) 
        {
            this.programmer = programmer;
            Version = 1.0;
            Changes = "First release";
        }
        public string Programmer
        { 
            get { return programmer; } 
        }
    }
    [History("Kang" , Version = 1.1 , Changes = "2025-6-17 Created class")]
    [History("Kang" , Version = 1.2 , Changes = "2025-6-17 Created class")]
    class MyClass
    {
        public void Func()
        {
            Console.WriteLine("Func()");
        }
    }
    internal class Attr
    {
        public static void A1() 
        {
            //어트리뷰트attribute
            //개발자가 쓰고 컴파일러가 읽어서 실행하는 주석 느낌
            //메타데이터 : 데이터의 데이터를 말한다.
            //C#코드도 데이터 이지만 이 코드에 대한 정보 데이터의 데이터를 메타 데이터라고 한다.
            MyClass2 mc = new MyClass2();
            mc.OldMethod();
            mc.NewMethod();
        }
        public static void A2() 
        {
            Trace.WriteLine("ㅁㄴㅇㄻㄴㅇㄹ");
        }
        public static void A3() 
        {
            Type type = typeof(MyClass);
            Attribute[] attributes = Attribute.GetCustomAttributes(type);
            Console.WriteLine("변화 과정");
            foreach (Attribute attr in attributes)
            {
                History h = attr as History;
                if(h != null)
                {
                    Console.WriteLine($"{h.Version} , {h.Programmer} , {h.Changes}");
                }
            }
        }
        public static void A4() { }
        public static void A5() { }
        public static void A6() { }
        public static void A7() { }
        public static void A8() { }
        public static void A9() { }
        public static void A10() { }

    }
}
