using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MUserSettings
{
    private static float time; //in seconds.
    private static string environ = "ocean";

    public static float getTime() {
        return time;
    }

    public static void setTime(float newTime)
    {
        time = newTime;
    }

    public static string getEnviron()
    {
        return environ;
    }

    public static void setEnviron(string newEnviron)
    {
        environ = newEnviron;
    }
}
