using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

    private SteamVR_TrackedObject Controller1;
	private SteamVR_TrackedObject Controller2;

    private bool controller1Inside = false;
    private bool controller2Inside = false;

	private void Awake()
	{
		SteamVR_ControllerManager manager = GameObject.Find("[CameraRig]").GetComponent<SteamVR_ControllerManager>();
		Controller1 = manager.left.GetComponent<SteamVR_TrackedObject>();
		Controller2 = manager.right.GetComponent<SteamVR_TrackedObject>();
	}

    private void Update()
    {
        CheckController(Controller1, controller1Inside);
        CheckController(Controller2, controller2Inside);
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
			}
        }
        if (ipt.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            print("RELEASED " + (int)controller.index);
        }
    }

    private bool IsController(Collider col)
    {
        print(col.gameObject);
        //TODO: check if it's a controller- tagged in prefab?
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsController(other))
        {
            controller1Inside = true; //TODO
            //TODO: highlight
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsController(other))
        {
			controller1Inside = false; //TODO
			//TODO: unhighlight
		}
    }
}
