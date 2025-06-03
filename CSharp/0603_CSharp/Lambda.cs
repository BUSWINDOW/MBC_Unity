using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

namespace _0603_CSharp
{
    #region 프로퍼티 예제
    class MyClass
    {
        public int myField { get; private set; } = 1;



        private int myField2;
        public int GetMyField() // getter함수
        {
            return myField2;
        }
        public void SetMyField(int value) // setter함수
        {
            myField2 = value;
        }

    }
    class YourClass
    {
        private int myField;
        public int MyField 
        {
            get { return myField; }
            set { myField = value; } 
        }

        public int myField2 { get; set; }
    }
    #endregion
    #region 프로퍼티 예제2
    class BirthdayInfo
    {
        private string name;
        private DateTime birthDay;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public DateTime BirthDay
        {
            get { return birthDay; }
            set { birthDay = value; }
        }
        public int Age
        {
            get { return new DateTime(DateTime.Now.Subtract(birthDay).Ticks).Year; }
        }
    }
    #endregion
    #region 자동 구현 프로퍼티
    public class NameCard
    {
        public string Name
        {

            get; set;
        }
        public string PhoneNumber
        {
            get; set;
        }
    }
    #endregion
    #region 인터페이스에서 프로퍼티 사용
    interface INamedValue
    {
        string Name { get; }
        string Value {  get;  }

        //인터페이스는 구현부가 없어서 이건 자동 완성 프로퍼티가 아님
    }
    class NamedValue : INamedValue 
    {
        public string Name { get; private set; }
        public string Value { get; private set; }
        public NamedValue(string name , string value) 
        {
            Name = name;
            Value = value;
        }

    }
    #endregion
    #region 추상 클래스에서의 프로퍼티 사용
    abstract class Product
    {
        private static int serial = 0;
        public string SerialID
        {
            get { return string.Format("{0:d5}" , serial++); }
        }
        abstract public DateTime ProductDate { get; set; }

    }
    class MyProduct : Product
    {
        
        public override DateTime ProductDate
        {
            get; set;
        }

    }
    #endregion

    #region 문제
    class NameCard_2
    {
        private int age;
        private string name;

        public int Age
        {
            get { return age; }
            set { age = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }


    #endregion
    internal class Lambda
    {
        public static void A1() 
        {
            MyClass obj = new MyClass();
            obj.SetMyField(1);
            Console.WriteLine($"rkqt : {obj.GetMyField()}");

            YourClass obj2 = new YourClass();
            obj2.MyField = 2;
            Console.WriteLine(obj2.MyField);

        }
        public static void A2() 
        {
            BirthdayInfo obj = new BirthdayInfo();
            obj.Name = "Test";
            obj.BirthDay = new DateTime(1557, 1, 1);
            Console.WriteLine($"{obj.Name}\n{obj.BirthDay}\n{obj.Age}");
        }
        public static void A3() 
        {
            NameCard obj = new NameCard();
            obj.Name = "Test";
            obj.PhoneNumber = "Test";
            Console.WriteLine($"{obj.Name} , {obj.PhoneNumber}");
        }
        public static void A4() 
        {
            NameCard nameCard = new NameCard()
            {
                Name = "Test",
                PhoneNumber = "Test"
            };
        }
        public static void A5() 
        {
            var obj =
                new
                {
                    Name = "asdf"

                };
            var b = 
                new 
            {
                Subject = "15", 
                Score = 57 
            };
            Console.WriteLine($"{b.Subject} : {b.Score}");
        }
        public static void A6() 
        {
            NamedValue namedValue = new NamedValue("asdf" , "asdf");
            
        }
        public static void A7() 
        {
            MyProduct product = new MyProduct();
            Console.WriteLine($"{product.SerialID}");
            Console.WriteLine($"{product.SerialID}");
            Console.WriteLine($"{product.SerialID}");
            Console.WriteLine($"{product.SerialID}");
            Console.WriteLine($"{product.SerialID}");
            Console.WriteLine($"{product.SerialID}");
            Console.WriteLine($"{product.SerialID}");
        }
        public static void A8()
        {
            var nameCard = new
            {
                Name = "박상현",
                Age = 17
            };

            Console.WriteLine("이름 : {0} , 나이 : {1}", nameCard.Name, nameCard.Age);

            var complex = new
            {
                Real = 3,
                Imaginary = -12
            };

            Console.WriteLine("Real : {0} , Imaginary : {1}" , complex.Real,complex.Imaginary);
        }
        public static void A9() { }
        public static void A10() { }
        public static void A11() { }
        public static void A12() { }
        public static void A13() { }
        public static void A14() { }
        public static void A15() { }
    }
}
