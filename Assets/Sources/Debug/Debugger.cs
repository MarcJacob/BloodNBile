using System;
using UnityEngine;

public static class Debugger
{
    static bool Activated = false;
    public static void TurnOff()
    {
        Activated = false;
    }

    public static void TurnOn()
    {
        Activated = true;
    }

    public static void LogMessage(object obj)
    {
        if (Activated == true)
        {
            Debug.Log(obj);
        }
    }
}

