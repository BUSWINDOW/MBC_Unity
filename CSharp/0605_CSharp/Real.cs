using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace _0605_CSharp
{
    internal class Real
    {
        public static void A1() 
        {
            int[,,] arr = new int[2, 3, 2]
            {
                {{ 1,3}, { 3,5}, { 6,78} },
                {{1,6}, { 1,7}, { 3,88} }
            };
            //왼쪽에서부터 크게 잡으면서 들어감
            //ex) 일단 2개짜리 배열 -> 그 배열안에 들어가 있는게 3개짜리 배열 -> 그 3개짜리 배열 안에 들어가 있는게 2개짜리 배열
            int[,] arr1 = new int[2, 3]
            {
                {1, 2, 3 },
                {4, 5, 6 }
            };
        }
        public static void A2() 
        {
            int[,] gugudan = new int[8, 9];
            for(int i = 0; i<gugudan.GetLength(0); i++)
            {
                for(int j = 0;j<gugudan.GetLength(1); j++)
                {
                    gugudan[i, j] = (i + 2) * (j + 1);
                    Console.Write($"{i+2} x {j+1} = {gugudan[i, j],3}  ");
                }
                Console.WriteLine();
            }
            

        }
        public static void A3() 
        {
            int[,,] arr = new int[2, 3, 4]
            {
                {{1,2,3,4 },
                 {5,6,7,8 },
                 {9,10,11,12 } },
                
                {{13,14,15,16 },
                 {17,18,19,20 },
                 {21,22,23,24 } }
            };

            foreach (int i in arr)
            {
                Console.WriteLine(i);
            }

            List<List<List<int>>> ints = new List<List<List<int>>>();
        }
        public static void A4() 
        {
            //대리자와 이벤트
            int a = 1;
            int b = 2;
            MyDel md = Calculator.Plus;

            Console.WriteLine(md(a, b));
            md = (a, b) => 
            { int sum = a - b;
                return sum;
            };
            Console.WriteLine(md(a, b));
        }
        delegate int MyDel(int a, int b);
        class Calculator
        {
            public static int Plus(int a ,int b)
            {
                return a + b;
            }
            public static int Minus(int a, int b)
            {
                return a - b;
            }

        }
        static void BubbleSort(int[] arr, Compare Compare)
        {
            int n = arr.Length;
            bool swapped; // 최적화를 위한 플래그: 한 번의 전체 순회 동안 스왑이 없으면 이미 정렬된 상태임



                for (int i = 0; i < n - 1; i++)
                {
                    swapped = false;
                    // 마지막 i개의 요소는 이미 정렬된 상태이므로, 내부 반복은 n-1-i 까지만 수행
                    for (int j = 0; j < n - 1 - i; j++)
                    {
                        // 현재 요소가 다음 요소보다 크면 (오름차순)
                        if (Compare(arr[j] ,arr[j + 1]))
                        {
                            // 두 요소의 위치를 바꿈 (스왑)
                            int temp = arr[j];
                            arr[j] = arr[j + 1];
                            arr[j + 1] = temp;
                            swapped = true; // 스왑이 발생했음을 표시
                        }
                    }

                    // 최적화: 내부 반복문에서 스왑이 한 번도 발생하지 않았다면,
                    // 배열이 이미 정렬된 상태이므로 더 이상 진행할 필요가 없음
                    if (swapped == false)
                    {
                        break;
                    }
                }
        }
        delegate bool Compare(int a, int b);
        static bool Accending(int a , int b)
        {
            return a > b;
        }
        static bool Deccending(int a, int b)
        {
            return a < b;
        }
        public static void A5() 
        {
            int[] arr = { 12, 3, 6, 56, 3, 24, 56, 3, 4, 23,1557, 5, 3, 5, 346, 34, 52 };
            foreach (int i in arr)
            {
                Console.Write($"{i,5}");
            }
            Console.WriteLine();
            BubbleSort(arr, Accending);
            foreach (int i in arr)
            {
                Console.Write($"{i,5}");
            }
            Console.WriteLine();
            BubbleSort(arr, Deccending);
            foreach (int i in arr)
            {
                Console.Write($"{i,5}");
            }
        }
        public static void A6() { }
        public static void A7() { }
        public static void A8() { }

    }
}
