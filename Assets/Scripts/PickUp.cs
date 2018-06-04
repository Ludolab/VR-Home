﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

    public Shader highlightShader;

    private SteamVR_TrackedObject controller1;
	private SteamVR_TrackedObject controller2;

    private bool controller1Inside = false;
    private bool controller2Inside = false;

	private SteamVR_TrackedObject holder = null;

	private Rigidbody rb;
    private Shader oldShader;
    private Renderer rend;

    private void Awake()
	{
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
    }
	
	private void Grab(SteamVR_TrackedObject controller)
	{
		if (holder == null)
		{
			holder = controller;
			gameObject.transform.parent = controller.gameObject.transform;
            rb.isKinematic = true;
        }
	}

	private void Release(SteamVR_TrackedObject controller)
	{
		if (holder == controller)
		{
			holder = null;
			gameObject.transform.parent = null;
            rb.isKinematic = false;
            SteamVR_Controller.Device input = GetInput(controller);
            rb.velocity = input.velocity;
            rb.angularVelocity = input.angularVelocity;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == controller1.gameObject)
        {
            controller1Inside = true;
            EnableHighlight();
        }
		if (other.gameObject == controller2.gameObject)
		{
			controller2Inside = true;
            EnableHighlight();
        }
	}

    private void OnTriggerExit(Collider other)
    {
		if (other.gameObject == controller1.gameObject)
		{
			controller1Inside = false;
            DisableHighlight();
        }
		if (other.gameObject == controller2.gameObject)
		{
			controller2Inside = false;
            DisableHighlight();
        }
	}

    private void EnableHighlight()
    {
        rend.material.shader = highlightShader;
    }

    private void DisableHighlight()
    {
        rend.material.shader = oldShader;
    }
}
