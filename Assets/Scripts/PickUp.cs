using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

    private SteamVR_TrackedObject controller1;
	private SteamVR_TrackedObject controller2;

    private bool controller1Inside = false;
    private bool controller2Inside = false;

	private SteamVR_TrackedObject holder = null;

	private Rigidbody rb;

	private void Awake()
	{
		SteamVR_ControllerManager manager = GameObject.Find("[CameraRig]").GetComponent<SteamVR_ControllerManager>();
		controller1 = manager.left.GetComponent<SteamVR_TrackedObject>();
		controller2 = manager.right.GetComponent<SteamVR_TrackedObject>();
		rb = GetComponent<Rigidbody>();
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
			//rb.enabled = false;
			rb.useGravity = false;
		}
	}

	private void Release(SteamVR_TrackedObject controller)
	{
		if (holder == controller)
		{
			holder = null;
			gameObject.transform.parent = null;
			//rb.enabled = true;
			rb.useGravity = true;
		}
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == controller1.gameObject)
        {
            controller1Inside = true;
            //TODO: highlight
        }
		if (other.gameObject == controller2.gameObject)
		{
			controller2Inside = true;
			//TODO: highlight
		}
	}

    private void OnTriggerExit(Collider other)
    {
		if (other.gameObject == controller1.gameObject)
		{
			controller1Inside = false;
			//TODO: unhighlight
		}
		if (other.gameObject == controller2.gameObject)
		{
			controller2Inside = false;
			//TODO: unhighlight
		}
	}
}
