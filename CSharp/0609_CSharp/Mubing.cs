using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0609_CSharp
{
    class MyList<T> :IEnumerable,IEnumerator
    {
        private T[] arr;
        int position = -1;
        public MyList()
        {
            arr = new T[3];
        }
        public T this[int index]
        {
            get
            {
                return arr[index];
            }
            set
            {
                if (index >= arr.Length)
                {
                    Array.Resize<T>(ref arr, index + 1);
                    Console.WriteLine("Resized");
                }
                arr[index] = value;
            }
        }
        public int Length
        {
            get { return arr.Length; }
        }

        public object Current
        {
            get
            {
                return arr[position];
            }
        }

        public IEnumerator GetEnumerator()
        {
            return this; // 현재 객체 반환
        }

        public bool MoveNext()
        {
            if(position == arr.Length - 1)
            {
                Reset(); // 마지막 요소에 도달하면 리셋 호출
                return false; // 더 이상 요소가 없으면 false 반환
            }
            position++;
            return position < arr.Length; // 현재 위치가 배열의 길이보다 작으면 true반환
        }

        public void Reset()
        {
            position = -1;
        }
    }
    class YourList<T> : IEnumerable<T>,IEnumerator<T>
    {
        private T[] arrF;
        int position = -1;
        public YourList()
        {
            this.arrF = new T[3];
        }
        public T this[int index]
        {
            get { return arrF[index]; }
            set
            {
                if (index >= arrF.Length) 
                {
                    Array.Resize<T>(ref this.arrF, index + 1);
                    Console.WriteLine("Resized");
                }
                arrF[index] = value;
            }
        }
        public int Length
        {
            get { return this.arrF.Length; }
        }

        public T Current
        {
            get { return arrF[position]; }
        }

        object IEnumerator.Current
        {
            get { return arrF[position]; }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            if(position == arrF.Length - 1)
            {
                Reset();
                return false;
            }
            position++;
            return position < arrF.Length;
        }

        public void Reset()
        {
            position = -1;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
    }

    class MyList_2 <T>: IEnumerable<T>, IEnumerator<T>
    {
        private T[] arr;
        int position = -1; //배열위치를 나타내는 변수 
        public MyList_2()
        {
            arr = new T[3];
        }
        public T this[int index] // 인덱서(property) 정의 
        {
            get
            {
                return arr[index];
            }
            set
            {
                if (index >= arr.Length)
                {
                    Array.Resize<T>(ref arr, index + 1); // 배열 크기 조정
                    Console.WriteLine($"Array Resized : {arr.Length}");
                }
                arr[index] = value; // 인덱스에 해당하는 값 설정

            }
        }
        public int Length
        {
            get { return arr.Length; }
        }

        public T Current
        {
            get { return arr[position]; }
        }

        object IEnumerator.Current
        {
            get { return arr[position]; }
        }

        public void Dispose()
        {

        }

        public IEnumerator<T> GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            if(this.position == arr.Length-1)
            {
                Reset();
                return false;
            }
            position++;
            return position < arr.Length;
        }

        public void Reset()
        {
            position = -1;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
    }
    internal class Mubing
    {
        public static void A1() 
        {
            ArrayList arrayList = new ArrayList();
            arrayList.Add(1);
            arrayList.Add(true);
            arrayList.Add("1557");
            foreach (var i in arrayList)
            {
                Console.WriteLine(i);
            }

        }
        public static void A2() 
        {
            MyList<int> list = new MyList<int>();
            for (int i = 0; i < 5; i++)
            {
                list[i] = i;
            }
            for (int i = 0;i < 5; i++)
            {
                Console.WriteLine(list[i]);
            }
        }
        public static void A3()
        {
            YourList<float> list = new YourList<float>();
            list[0] = 1.1f;
            list[1] = 2.1f;
            list[2] = 3.1f;
            list[3] = 4.1f;
            list[4] = 5.1f;

            for(int i = 0;i<list.Length ; i++)
            {
                Console.WriteLine(list[i]);
            }
        }
        public static void A4() 
        {
            /*MyList list = new MyList();
            for(int i = 0;i<5 ; i++)
            {
                list[i] = i + 1;
            }
            foreach (var i in list) 
            {
                Console.WriteLine(i);
            }*/
        }
        public static void A5() 
        {
            int[] source = { 1, 2, 3, 4, 5 };
            int[] target = new int[source.Length];
            CopyArray(source, target);
            Console.WriteLine("int 복사");
            foreach(var a in target)
            {
                Console.WriteLine(a);
            }

        }
        static void CopyArray<T>(T[] source, T[] target)
        {
            for (int i = 0; i<source.Length ; i++)
            {
                target[i] = source[i];
            }
        }
        static void CopyArray(string[] source , string[] target)
        {
            for(int i = 0;i< source.Length ; i++)
            {
                target[i] = source[i];
            }
        }
        public static void A6() 
        {
            MyList<string> str_list = new MyList<string>();
            str_list[0] = "asdf";
            str_list[1] = "asdfasdf";
            str_list[2] = "asdfsdf";
            str_list[3] = "asdfasdfasdf";
            for (int i = 0;i<str_list.Length ; i++) 
            {
                Console.WriteLine(str_list[i]);
            }

            Console.WriteLine();

            MyList<int> int_list = new MyList<int>();
            for (int i = 0;i< int_list.Length ; i++)
            {
                int_list[i] = i+1;
            }

            foreach (int i in int_list)
            {
                Console.WriteLine(i);
            }
        }
        public static void A7() 
        {
            MyList_2<int> int_list = new MyList_2<int>();
            for(int i = 0; i< 5 ; i++)
            {
                int_list[i] = i + 1;
            }
            foreach (int i in int_list)
            {
                Console.WriteLine(i);
            }
        }
        public static void A8() { }
        public static void A9() { }
        public static void A10() { }

    }
}

