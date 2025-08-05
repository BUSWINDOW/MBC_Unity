using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace _0729_ReflectionTest
{
    interface BaseMsgHandler                    // 최상위 클래스. 이 클래스를 상속 받는 모든 클래스를 찾을 것이다.
    {
    }

    class ConcreteMsgHandler_1 : BaseMsgHandler // 찾을 대상 클래스 1
    {
    }

    class ConcreteMsgHandler_2 : BaseMsgHandler // 찾을 대상 클래스 2
    {
    }

    class ConcreteMsgHandler_3 : BaseMsgHandler // 찾을 대상 클래스 3
    {
    }

    class Other_1                               // 찾을 대상이 아닌 클래스 1
    {
    }

    class Other_2                               // 찾을 대상이 아닌 클래스 2
    {
    }

    internal class Program
    {
        static List<BaseMsgHandler> dispatcher = new List<BaseMsgHandler>();
        static void Main(string[] args)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain; // 현재 어플리케이션 도메인
            Assembly[] assems = currentDomain.GetAssemblies(); // 현재 어플리케이션 도메인에 로드 된 모든 어셈블리를 가져온다
            IEnumerable<Assembly> currentAssembly = assems.Where(a => a.GetName().Name.Equals("helloworld_cs"));
            IEnumerable<Type> childrenTypes = currentAssembly.SelectMany(s => s.GetTypes()).Where(p => typeof(BaseMsgHandler).IsAssignableFrom(p) && p.IsClass);
            foreach (var type in childrenTypes)
            {
                BaseMsgHandler handler = Activator.CreateInstance(type) as BaseMsgHandler;
                dispatcher.Add(handler); // 디스패처에 핸들러 등록
            }
        }
    }
}
