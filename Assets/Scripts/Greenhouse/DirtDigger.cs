using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

public class DirtDigger : MonoBehaviour
{

    public Dirt myDirt;
    public float motionThreshold;
    InteractionBehaviour myIB;

	private void Start()
	{
        myIB = this.gameObject.GetComponent<InteractionBehaviour>();
	}

	public void handTouching()
    {
        InteractionController myController = myIB.closestHoveringController;
        Vector3 myPos = this.gameObject.transform.position - new Vector3(0, 0.001f, 0);
        Vector3 relativePos = myPos - myController.transform.position;
        Vector3 vel = myController.velocity;
        float relativeAngle = Vector3.Angle(relativePos, vel);
        Debug.Log("relative Angle is " + relativeAngle);
        if (relativeAngle > 90 && vel.magnitude > motionThreshold)
        {
            Debug.Log("Telling to Dig");
            StartCoroutine(myDirt.DigHole());
        }
        if (relativeAngle < 60 && vel.magnitude > motionThreshold)
        {
            Debug.Log("Telling to Cover");
            StartCoroutine(myDirt.CoverHole());
        }
    }
}