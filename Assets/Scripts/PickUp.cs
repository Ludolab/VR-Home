using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{

    //TODO: make this work for two controllers- make controller-specific stuff arrays of len 2
    private static GameObject grabbableObject = null;

    public Shader highlightShader;
    public Color heldColor;
    public Color hoverColor;

    private SteamVR_TrackedObject controller1;
    private SteamVR_TrackedObject controller2;

    private bool controller1Inside = false;
    private bool controller2Inside = false;

    private SteamVR_TrackedObject holder = null;

    private Rigidbody rb;
    private Shader oldShader;
    private Renderer rend;

    private Vector3 startPos;

    private void Start()
    {
        startPos = gameObject.transform.position;
        SteamVR_ControllerManager manager = GameObject.Find("[CameraRig]").GetComponent<SteamVR_ControllerManager>();
        controller1 = manager.left.GetComponent<SteamVR_TrackedObject>();
        controller2 = manager.right.GetComponent<SteamVR_TrackedObject>();
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        oldShader = rend.material.shader;
    }

    private SteamVR_Controller.Device GetInput(SteamVR_TrackedObject controller)
    {
        return SteamVR_Controller.Input((int)controller.index);
    }

    private void Update()
    {
        CheckController(controller1, controller1Inside);
        CheckController(controller2, controller2Inside);
    }

    private void CheckController(SteamVR_TrackedObject controller, bool inside)
    {
        try
        {
            SteamVR_Controller.Device input = GetInput(controller);
            if (input.GetHairTriggerDown())
            {
                if (inside)
                {
                    Grab(controller);
                }
            }
            if (input.GetHairTriggerUp())
            {
                Release(controller);
            }

            if (input.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
            {
                Reset();
            }
        }
        catch (IndexOutOfRangeException)
        {
            //can't talk to controller, don't do anything
        }
    }

    private bool CanGrab()
    {
        return grabbableObject == gameObject && holder == null;
    }

    private void Grab(SteamVR_TrackedObject controller)
    {
        if (CanGrab())
        {
            holder = controller;
            gameObject.transform.parent = controller.gameObject.transform;
            rb.isKinematic = true;
            SetColor(heldColor);
        }
    }

    private void Release(SteamVR_TrackedObject controller)
    {
        if (holder == controller)
        {
            grabbableObject = null;
            holder = null;
            gameObject.transform.parent = null;
            rb.isKinematic = false;
            SteamVR_Controller.Device input = GetInput(controller);
            rb.velocity = input.velocity;
            rb.angularVelocity = input.angularVelocity;
            SetGrabbable();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ActionIfBowl(other, b => b.AddObject());
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == controller1.gameObject)
        {
            controller1Inside = true;
            SetGrabbable();
        }
        if (other.gameObject == controller2.gameObject)
        {
            controller2Inside = true;
            SetGrabbable();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == controller1.gameObject)
        {
            controller1Inside = false;
        }
        if (other.gameObject == controller2.gameObject)
        {
            controller2Inside = false;
        }
        if (!controller1Inside && !controller2Inside)
        {
            SetNotGrabbable();
        }

        ActionIfBowl(other, b => b.RemoveObject());
    }

    private void ActionIfBowl(Collider other, Action<Bowl> action)
    {
        Transform parent = other.gameObject.transform.parent;
        if (parent != null)
        {
            Bowl bowl = parent.gameObject.GetComponent<Bowl>();
            if (bowl != null)
            {
                action(bowl);
            }
        }
    }

    private void SetGrabbable()
    {
        if (grabbableObject == null)
        {
            grabbableObject = gameObject;
        }

        if (CanGrab())
        {
            rend.material.shader = highlightShader;
            SetColor(hoverColor);
        }
    }

    private void SetNotGrabbable()
    {
        if (grabbableObject == gameObject)
        {
            grabbableObject = null;
        }

        rend.material.shader = oldShader;
    }

    private void SetColor(Color color)
    {
        rend.material.SetColor("_OutlineColor", color);
    }

    private void Reset()
    {
        if (holder != null)
        {
            Release(holder);
        }
        gameObject.transform.position = startPos;
        rb.velocity = Vector3.zero;
    }
}
