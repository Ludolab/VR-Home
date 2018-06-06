using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserSettings
{
    private static float time; //in seconds.

    public static float getTime() {
        return time;
    }

    public static void setTime(float newTime) {
        time = newTime;
    }
}
