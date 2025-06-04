using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace _0604_CSharp
{
    internal class Tian
    {
        public static bool CheckPassed(int score)
        {
            return score >= 60;
        }
        public static void PrintScores(int value)
        {
            Console.WriteLine(value);
        }
        public static void A1() 
        {
            List<int> numbers = new List<int>();
            Dictionary<int, string> stringDic = new Dictionary<int, string>();
            stringDic.Add(1557, "Fast");
            Console.WriteLine(stringDic[1557]);


            int[] scores = new int[5];
            for(int i = 0; i<scores.Length; i++)
            {
                scores[i] = int.Parse(Console.ReadLine());
            }
            foreach (int i in scores)
                Console.WriteLine(i);

            int sum = 0;
            for(int i = 0;i<scores.Length; i++)
            {
                sum += scores[i];
            }
            Console.WriteLine($"점수 합계 : {sum}");
            int average = sum / scores.Length;
            Console.WriteLine($"점수 평균 : {average}");

            foreach (var a in stringDic)
            {
                Console.WriteLine(a);
            }
        }
        public static void A2() 
        {
            string[] array1 = new string[3] { "apple" , "banana" , "cherry"};
            string[] array2 = new string[] { "apple", "banana", "cherry" };
            string[] array3 = { "apple", "banana", "cherry" };

            foreach (var a in array1)
                Console.WriteLine(a);
            foreach (var b in array2)
                { Console.WriteLine(b); }
            foreach (var c in array3)
                { Console.WriteLine(c); }
        }
        public static void A3() 
        {
            int[] array = new int[5] { 1, 2, 3, 4, 5 };
            Console.WriteLine(array.GetType());
            Console.WriteLine(array.GetType().BaseType);


        }
        public static void A4() 
        {
            int[] arr = { 123,4,24536,3567,23,5,4758,45,7,45,34,54,56,423,6,4568,58,9,562,546 };
            //이진탐색 알고리즘 : 일단 정렬이 되어있어야함
            Console.WriteLine(BSearch(arr, 45));
        }
        public static int LSearch(int[] arr, int len, int target)
        {
            for (int i = 0; i < len; i++) 
            {
                if (arr[i] == target)
                {
                    return i;
                }
            }
            return -1;
        }
        public static int BSearch(int[] arr, int target)
        {
            Array.Sort(arr);

            int first = 0;
            int last = arr.Length - 1;
            int mid;
            while (first <= last)
            {
                mid = first + (last - first) / 2;
                if (arr[mid] == target)
                {
                    return mid;
                }
                else if (arr[mid] < target)
                {
                    first = mid + 1;
                }
                else
                {
                    last = mid - 1;
                }

            }
            return -1;
        }
        public static int BSearch_noSort(int[] arr, int target)
        {

            int first = 0;
            int last = arr.Length - 1;
            int mid;
            while (first <= last)
            {
                mid = first + (last - first) / 2;
                if (arr[mid] == target)
                {
                    return mid;
                }
                else if (arr[mid] < target)
                {
                    first = mid + 1;
                }
                else
                {
                    last = mid - 1;
                }

            }
            return -1;
        }
        public static void A5() 
        {
            int[] arr = { 123, 4, 24536, 3567, 23, 5, 4758, 45, 7, 45, 34, 54, 56, 423, 6, 4568, 58, 9, 562, 546 };
            foreach (int i in arr)
            {
                Console.WriteLine(i);
            }
            BubbbleSort(arr);
            Console.WriteLine();
            foreach(int i in arr)
            {
                Console.WriteLine(i);
            }
            /*int index = Array.FindIndex<int>(arr, score => score < 60);
            Console.WriteLine(index);*/
        }
        public static void BubbbleSort(int[] arr)
        {
            int n = arr.Length;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j< n-1-i; j++)
                {
                    if (arr[j+1] < arr[j])
                    {
                        int temp = arr[j+1];
                        arr[j+1] = arr[j];
                        arr[j] = temp;
                    }
                }
            }
        }
        public static void A6() 
        {
            int[] arr = new int[100];
            Random rand = new Random();
            for (int i = 0; i< arr.Length; i++)
            {
                arr[i] = rand.Next(1, 100);
            }
            Console.WriteLine(BSearch(arr, 45));

            BubbbleSort(arr);

            Console.WriteLine(BSearch_noSort(arr,45));
        }
        public static void A7() { }
        public static void A8() { }
        public static void A9() { }
        public static void A10() { }
    }
}
