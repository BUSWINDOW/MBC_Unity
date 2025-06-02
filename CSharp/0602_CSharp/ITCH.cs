using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0602_CSharp
{
    #region 인터페이스 예제 1
    interface ILogger // 메서드나 속성, 이벤트 등을 정의 해두는 곳
                      // 구현은 여기서 못함
    {
        void WriteLog(string message);
    }
    class ConsoleLogger : ILogger
    {
        public void WriteLog(string message)
        {
            Console.WriteLine($"{DateTime.Now.ToLocalTime},{message}");
        }
    }

    class FileLogger : ILogger
    {
        private StreamWriter _writer;
        public FileLogger(string path)
        {
            _writer = File.CreateText(path); // 파일을 생성하고 스트림 라이터를 초기화
            _writer.AutoFlush = true; // 자동으로 버퍼를 비우도록 설정
        }
        public void WriteLog(string message)
        {
            Console.WriteLine($"File log : {DateTime.Now.ToShortTimeString()} , {message}");
            _writer.WriteLine($"File log : {DateTime.Now.ToShortTimeString()} , {message}");
        }
    }
    class ClimateMonitor
    {
        private ILogger logger;
        public ClimateMonitor(ILogger logger)
        {
            this.logger = logger;
        }
        public void Start()
        {
            while (true)
            {
                Console.Write("온도 입력 : ");
                string temperInput = Console.ReadLine();
                if (temperInput == "")
                {
                    break;
                }
                logger.WriteLog($"현재 온도 : {temperInput}");
            }
        }
    }
    #endregion
    #region 인터페이스 예제 2) 인터페이스를 상속하는 인터페이스
    interface ILogger2
    {
        void WriteLog(string message);
    }
    interface IFormattableLogger : ILogger2
    {
        void WriteLog(string message, params Object[] args);
    }
    class ConsoleLogger2 : IFormattableLogger
    {
        public void WriteLog(string message)
        {
            Console.WriteLine($"{DateTime.Now.ToLocalTime()}, {message}");
        }
        public void WriteLog(string format, params object[] args)
        {
            string message = string.Format(format, args);
            Console.WriteLine($"{DateTime.Now.ToLocalTime()}, {message}");
        }
    }
    #endregion
    #region 인터페이스 예제 3) 다중상속
    interface IRunnable
    {
        void Run();
    }
    interface IWalkable
    {
        void Walk();
    }
    class Human : IWalkable, IRunnable
    {
        public void Walk() 
        {
            Console.WriteLine("슝");
        }
        public void Run() 
        {
            Console.WriteLine("좍");
        }
    }
    #endregion
    #region 인터페이스 예제 4) 인터페이스 내부 구현?
    interface ILogger4
    {
        void WriteLog(string message);
        void WriteError(string message)
        {
            WriteLog($"에러 : {message}");
        }
    }
    class ConsoleLogger4 : ILogger4
    {
        public void WriteLog(string message)
        {
            Console.WriteLine(message);
        }
        public void WriteError(string message)
        {
            Console.Write(message);
        }
    }
    #endregion
    #region 추상 클래스
    //추상 클래스는 구현도 포함 가능
    public abstract class AbstractBase
    {
        protected void PrivateMethodA()
        {
            Console.WriteLine("A called");
        }
        public void PublicMethodA()
        {
            Console.WriteLine("Public A called");
        }
        public abstract void AbstractMethodA(); // 추상 메서드는 구현부가 없음,구현하려하면 에러남
                                                // 추상 메서드 하나라도 포함 시 클래스도 추상 클래스 여야함

    }

    class Derived : AbstractBase
    {
        public override void AbstractMethodA()
        {
            Console.WriteLine("Derived Abstract A called");
        }
        
    }
    #endregion
    internal class ITCH
    {
        public static void A1()
        {
            ILogger logger = new FileLogger("C:\\Users\\WD\\Desktop\\KYS\\MBC_Unity\\CSharp\\0602_CSharp\\MyLog.txt");
            ClimateMonitor monitor = new ClimateMonitor(logger);
            monitor.Start();
        }
        public static void A2() 
        {
            ConsoleLogger2 logger = new ConsoleLogger2();
            logger.WriteLog("123");
            logger.WriteLog("{0} , {1} , {2} , {3}", 1, "5", 2+3, 7.0);
        }
        public static void A3() 
        {
            Human human = new Human();
            human.Walk();
            human.Run();
            IWalkable walkable = human as IWalkable;
            walkable.Walk();
            IRunnable runnable = human as IRunnable;
            runnable.Run();
        }
        public static void A4() 
        {
            ILogger4 logger = new ConsoleLogger4();
            logger.WriteLog("123");
            logger.WriteError("1234");
        }
        public static void A5() 
        {
            //AbstractBase baseA = new AbstractBase();
            AbstractBase baseB = new Derived();
            baseB.AbstractMethodA();
            baseB.PublicMethodA();
            //baseB.PrivateMethodA();
        }
        public static void A6() { }
        public static void A7() { }
        public static void A8() { }
        public static void A9() { }
        public static void A10() { }
    }
}

