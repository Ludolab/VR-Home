using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRUIPointerVisual : SteamVR_LaserPointer {

    public LineRenderer visual;

    private LineRenderer pointerVisual;

	// Use this for initialization
	void Start () {
        pointerVisual = Instantiate(visual);
        pointerVisual.transform.parent = holder.transform;
        pointerVisual.transform.localScale = new Vector3(thickness, thickness, 100f);
        pointerVisual.transform.localPosition = new Vector3(0f, 0f, 50f);
        pointerVisual.transform.localRotation = Quaternion.identity;
	}
	
	// Update is called once per frame
    void Update () {
        float dist = 100f;
        SteamVR_TrackedController controller = GetComponent<SteamVR_TrackedController>();

        if (bHit && hit.distance < 100f)
        {
            dist = hit.distance;
        }

        if (controller != null && controller.triggerPressed)
        {
            pointerVisual.transform.localScale = new Vector3(thickness * 2f, thickness * 2f, dist);
        }
        else
        {
            pointerVisual.transform.localScale = new Vector3(thickness, thickness, dist);
        }
        pointerVisual.transform.localPosition = new Vector3(0f, 0f, dist / 2f);
	}
}
