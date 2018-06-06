using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCamera : MonoBehaviour {

    public float mouseSensitivity = 4.0f;

    private const float MIN_VERT_ROTATION = -90;
    private const float MAX_VERT_ROTATION = 90;
    private const float CAMERA_CHECK_TIME = 0.5f;

    private Camera cam;
    private float horizRotation = 0;
    private float vertRotation = 0;

    private void Start()
    {
        StartCoroutine(CameraCheck());
	}
	
    private IEnumerator CameraCheck()
    {
        yield return new WaitForSeconds(CAMERA_CHECK_TIME);
        cam = gameObject.GetComponent<Camera>();
		if (VRFailed())
        {
            TakeControl();
        }
    }

    private bool VRFailed()
    {
        //TODO: does this work if enabled?
        GameObject camObj = Camera.main.gameObject;
        SteamVR_TrackedObject camScript = camObj.GetComponent<SteamVR_TrackedObject>();
        return camScript == null;
    }

    private void TakeControl()
    {
        print("No VR detected- mouse control override");
        cam.tag = "MainCamera";
        while (Camera.main != cam)
        {
            Camera.main.enabled = false;
        }
    }

    private void Update()
    {
        //TODO: Turn camera with mouse movement side/side
        //TODO: Turn camera up/down with mouse movement up/down (min -90, max 90)
        float horizTurn = mouseSensitivity * Input.GetAxis("Mouse X");
        float vertTurn = mouseSensitivity * -Input.GetAxis("Mouse Y");
        horizRotation += horizTurn;
        vertRotation += vertTurn;
        vertRotation = Mathf.Clamp(vertRotation, MIN_VERT_ROTATION, MAX_VERT_ROTATION);
        /*Vector3 rotation = new Vector3(vertTurn, horizTurn, 0);
        gameObject.transform.Rotate(rotation, Space.Self);
        print(transform.eulerAngles);*/
        transform.eulerAngles = new Vector3(vertRotation, horizRotation, 0);
    }
}
