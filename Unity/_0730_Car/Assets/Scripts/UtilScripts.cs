using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilScript
{

    public static IEnumerator WaitForSeconds(Action act, float seconds)
    {
        // 몇초 기다리는 정적 메서드
        yield return new WaitForSeconds(seconds);
        act();
    }
    public static IEnumerator WaitForBool(Action act, Func<bool> Bool)
    {
        yield return new WaitWhile(Bool);
        act();
    }
}
