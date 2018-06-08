using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaylightBowl : Bowl {

    public GameObject Sky;

    private SetSky skyScript;

    private void Start()
    {
        skyScript = Sky.GetComponent<SetSky>();
    }

    protected override float GetValue()
    {
        return skyScript.percentThroughDay / 100;
    }

    protected override void SetValue(float newValue)
    {
        skyScript.percentThroughDay = newValue * 100;
    }
}
