using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : MonoBehaviour
{

    private const int NUM_CONTROLLERS = 2;


    protected GameObject heldObject = null;

    void Start()
    {

    }

    /*protected virtual void Update()
    {
        for (int controllerIndex = 0; controllerIndex < NUM_CONTROLLERS; controllerIndex++)
        {
            CheckController(controllerIndex);
        }
    }

    private void CheckController(int controllerIndex)
    {
        try
        {
            SteamVR_Controller.Device input = GetInput(controllerIndex);
            if (input.GetHairTriggerDown())
            {
                if (CanGrab(controllerIndex))
                {
                    Grab(controllerIndex);
                }
            }

            if (input.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {
                Reset();
            }
        }
        catch (IndexOutOfRangeException)
        {
            //can't talk to controller, don't do anything
        }
    }*/

    public virtual void Apply(GameObject obj)
    {
        heldObject = obj;
        GetComponent<Renderer>().material.mainTexture = obj.GetComponent<Renderer>().material.mainTexture;
    }
}
