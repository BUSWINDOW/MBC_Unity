using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0616_CSharp
{
    class Profile
    {
        public string Name { get; set; }
        public int Height { get; set; }

    }
    class Class
    {
        public string Name { get; set; }
        public int[] Scores { get; set; }
    }
    class Car
    {
        public int Cost { get; set; }
        public int MaxSpeed { get; set; }
    }
    class Product
    {
        public string Title { get; set; }
        public string Star {  get; set; }
    }
    internal class Curry
    {
        public static void A1()
        {
            Profile[] profiles =
            {
                new Profile(){Name = "asdf" , Height = 1557},
                new Profile(){Name = "sdfg" , Height = 1601},
                new Profile(){Name = "dfgh" , Height = 15571601},
                new Profile(){Name = "jghkl" , Height = 88848},
                new Profile(){Name = "qwer" , Height = 888484}
            };

            List<Profile> listProf = new List<Profile>();
            foreach (Profile prof in profiles)
            {
                if (prof.Height > 1601)
                {
                    listProf.Add(prof);
                }

            }
            listProf.Sort(
                (profile1, profile2) =>
                {

                    return profile2.Height - profile1.Height;
                }
            );


            foreach (Profile prof in listProf)
            {
                Console.WriteLine($"{prof.Name} , {prof.Height}");
            }

        }
        public static void A2()
        {
            //Linq를 써서 위에 적은 내용(A1) 구현
            Profile[] arrProfile =
            {
                new Profile(){Name = "asdf" , Height = 1557},
                new Profile(){Name = "sdfg" , Height = 1601},
                new Profile(){Name = "dfgh" , Height = 15571601},
                new Profile(){Name = "jghkl" , Height = 88848},
                new Profile(){Name = "qwer" , Height = 888484}
            };

            var profiles = from profile in arrProfile //어디서 값을 가져오는가(in) , 가져온 값의 이름을 뭘로 쓸건가(from)
                           where profile.Height > 1601 // 어떤 조건으로 가져오는가
                           orderby profile.Height ascending //어떤걸 기준으로 정렬하는가,
                                                            //뒤에 asc dec등을 붙여서 오름차순인지 내림차순인지
                           select profile; //그 기준대로 값을 선택 , 쿼리문의 마지막이 select가 아니면 에러

            foreach (Profile prof in profiles)
            {
                Console.WriteLine($"{prof.Name}, {prof.Height}");
            }


        }
        public static void A3()
        {
            List<int> ints = new List<int>();
            Random r = new Random();
            for (int i = 0; i < 10; i++)
            {
                ints.Add(r.Next(1, 101));
            }
            foreach (int i in ints)
            {
                Console.WriteLine(i);
            }

            Console.WriteLine("\n\n\n");
            var intQ = from inte in ints
                       where inte % 2 == 0
                       orderby inte descending
                       select inte;

            foreach (int i in intQ)
            {
                Console.WriteLine(i);
            }
        }
        public static void A4()
        {
            Profile[] arrProfile =
              {
    new Profile(){Name="정우성",Height =186},
    new Profile(){Name="김신영",Height =158},
    new Profile(){Name="박은빈",Height =172},
    new Profile(){Name="이정재",Height =178},
    new Profile(){Name="하하" ,Height =171}
            };

            var prof = from profile in arrProfile
                       where profile.Height >= 175
                       orderby profile.Height
                       select new { Name = profile.Name, Height = profile.Height * 0.393f };

            foreach (var p in prof)
            {
                Console.WriteLine($"{p.Name} , {p.Height}");
            }
        }
        public static void A5()
        {
            Class[] arrClass =
            {
                new Class(){ Name = "연두반" , Scores = new int[]{99,80,70,24 } },
                new Class(){ Name = "ㄱ" , Scores = new int[]{123,234,456,698 } },
                new Class(){ Name = "ㄴ" , Scores = new int[]{34,56,723,7 } },
                new Class(){ Name = "ㄷ" , Scores = new int[]{435,634,456,345 } },

            };

            var classes = from cls in arrClass
                          from s in cls.Scores
                          where s < 200
                          select new { Name = cls.Name, Score = s };
            foreach (var s in classes)
            {
                Console.WriteLine($"{s.Name} , {s.Score}");
            }
        }
        public static void A6()
        {
            Profile[] arrProfile =
  {
    new Profile(){Name="정우성",Height =186},
    new Profile(){Name="김신영",Height =158},
    new Profile(){Name="박은빈",Height =172},
    new Profile(){Name="이정재",Height =178},
    new Profile(){Name="하하" ,Height =171}
            };
            var listProfile = from prof in arrProfile
                              orderby prof.Height
                              group prof by prof.Height < 175 into g
                              select new { GroupKey = g.Key, Profile = g };

            foreach (var prof in listProfile)
            {
                Console.WriteLine($"{prof.GroupKey} , {prof.Profile}");
                foreach (var a in prof.Profile)
                {
                    Console.WriteLine($"{a.Name} , {a.Height}");
                }
            }
        }
        public static void A7()
        {
            Car[] cars =
            {
                new Car(){Cost = 56, MaxSpeed = 120},
                new Car(){Cost = 70, MaxSpeed = 150},
                new Car(){Cost = 45, MaxSpeed = 180},
                new Car(){Cost = 32, MaxSpeed = 200},
                new Car(){Cost = 82, MaxSpeed = 280}
            };

            var selected = from car in cars
                           where car.Cost >= 50 && car.MaxSpeed >= 150
                           select car;
            foreach (var car in selected)
            {
                Console.WriteLine($"{car.Cost},{car.MaxSpeed}");
            }
        }
        public static void A8()
        {
            Class[] arrClass =
{
     new Class(){Name ="연두반",Scores = new int[] {99,80,70,24} },
     new Class(){Name ="분홍반",Scores = new int[] {60,45,87,72} },
     new Class(){Name ="파랑반",Scores = new int[] {92,30,85,94} },
     new Class(){Name ="노랑반",Scores = new int[] {90,88,0,17} }
 };
            var classes = from cls in arrClass
                          from score in cls.Scores
                          group cls by score >= 60 into ss
                          select new { GroupKey = ss.Key, value = ss };

            foreach (var cls in classes)
            {
                Console.WriteLine(cls.GroupKey ? "합격" : "불합격");
                foreach (var a in cls.value)
                {
                    Console.WriteLine($"{a.Name} , {a.Scores}");
                }
            }


            var groupedClasses = from cls in arrClass
                                 from score in cls.Scores
                                 group new { cls.Name, Score = score } by score >= 60 into ss
                                 select new { GroupKey = ss.Key, Values = ss.ToList() };

            foreach (var group in groupedClasses)
            {
                Console.WriteLine($"--- {(group.GroupKey ? "60점 이상" : "60점 미만")} ---");
                foreach (var item in group.Values)
                {
                    Console.WriteLine($"이름: {item.Name}, 점수: {item.Score}");
                }
            }




        }
        public static void A9()
        {
            Profile[] arrProfile =
            {
                new Profile(){Name="정우성",Height =186},
                new Profile(){Name="김신영",Height =158},
                new Profile(){Name="박은빈",Height =172},
                new Profile(){Name="이정재",Height =178},
                new Profile(){Name="하하" ,Height =171}
            };
            Product[] arrProduct =
            {
                new Product(){Title ="서울의 봄", Star ="정우성"},
                new Product(){Title ="CF 다수", Star ="박은빈"},
                new Product(){Title ="이상한변호사 우영우", Star ="박은빈"},
                new Product(){Title ="코미디 빅리그", Star ="김신영"},
                new Product(){Title ="오징어게임", Star ="이정재" }
            };

            var ProfJoinProd = from prof in arrProfile
                               join prod in arrProduct on prof.Name equals prod.Star
                               select new { Name = prof.Name , Title = prod.Title };
            
            foreach (var prof in ProfJoinProd)
            {
                Console.WriteLine($"{prof.Name} , {prof.Title}");
            }

            Console.WriteLine("\n\n\n");

            var ProfJoinProd2 = from prof in arrProfile
                               join prod in arrProduct on prof.Name equals prod.Star into ps
                               from prod in ps.DefaultIfEmpty(new Product() {Title = "없음" })
                               select new { Name = prof.Name, Title = prod.Title };

            foreach (var prof in ProfJoinProd2)
            {
                Console.WriteLine($"{prof.Name} , {prof.Title}");
            }

            Console.Write("\n\n\n");

            var heightStat = from prof in arrProfile
                             group prof by prof.Height < 175 into g
                             select new
                             {
                                 Group = g.Key == true ? "175미만" : "175이상",
                                 Count = g.Count(),
                                 Max = g.Max(prof => prof.Height),
                                 Min = g.Min(prof => prof.Height),
                                 Avg = g.Average(prof => prof.Height)

                             };

            foreach (var prof in heightStat)
            {
                Console.WriteLine(prof);
            }


        }
        public static void A10() 
        {
            Car[] cars =
            {
                new Car(){Cost = 56, MaxSpeed = 120},
                new Car(){Cost = 70, MaxSpeed = 150},
                new Car(){Cost = 45, MaxSpeed = 180},
                new Car(){Cost = 32, MaxSpeed = 200},
                new Car(){Cost = 82, MaxSpeed = 280}
            };

            var carss = from car in cars

                        select new
                        {
                            Count = cars.Count(),
                            Max = cars.Max(car => car.Cost),
                            Min = cars.Min(car => car.Cost),
                            Avg = cars.Average(car => car.Cost)
                        };

            var test = new
            {
                Count = cars.Count(),
                Max = cars.Max(car => car.Cost),
                Min = cars.Min(car => car.Cost),
                Avg = cars.Average(car => car.Cost)
            };

            Console.WriteLine(test);
        }
        public static void A11() { }
        public static void A12() { }
        public static void A13() { }
        public static void A14() { }
        public static void A15() { }

    }
}
