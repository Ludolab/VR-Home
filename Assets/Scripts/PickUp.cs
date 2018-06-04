using System.Collections;
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
    private Renderer renderer;

    private void Awake()
	{
		SteamVR_ControllerManager manager = GameObject.Find("[CameraRig]").GetComponent<SteamVR_ControllerManager>();
		controller1 = manager.left.GetComponent<SteamVR_TrackedObject>();
		controller2 = manager.right.GetComponent<SteamVR_TrackedObject>();
		rb = GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();
        oldShader = renderer.material.shader;
    }

    private void Update()
    {
        CheckController(controller1, controller1Inside);
        CheckController(controller2, controller2Inside);
    }

    private void CheckController(SteamVR_TrackedObject controller, bool inside)
    {
        var ipt = SteamVR_Controller.Input((int)controller.index);
        if (ipt.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            print("PRESSED " + (int)controller.index);
			if (inside)
			{
				print("PICKUP");
				Grab(controller);
			}
        }
        if (ipt.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            print("RELEASED " + (int)controller.index);
			Release(controller);
        }
    }
	
	private void Grab(SteamVR_TrackedObject controller)
	{
		if (holder == null)
		{
			holder = controller;
			gameObject.transform.parent = controller.gameObject.transform;
            //rb.useGravity = false;
            rb.isKinematic = true;
        }
	}

	private void Release(SteamVR_TrackedObject controller)
	{
		if (holder == controller)
		{
			holder = null;
			gameObject.transform.parent = null;
            //rb.useGravity = true;
            rb.isKinematic = false;
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
        renderer.material.shader = highlightShader;
    }

    private void DisableHighlight()
    {
        renderer.material.shader = oldShader;
    }
}
