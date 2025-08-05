using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilScripts
{
    public static IEnumerator WaitForSec(Action act, float sec)
    {
        yield return new WaitForSeconds(sec);
        act();
    }
}
