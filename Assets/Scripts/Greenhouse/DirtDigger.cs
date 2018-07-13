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
        Debug.Log("Collision with Dirt");
        GameObject myController = myIB.closestHoveringController.gameObject;
        Vector3 relativePos = this.gameObject.transform.position - myController.transform.position;
        Vector3 vel = myController.GetComponent<Rigidbody>().velocity;
        float relativeAngle = Vector3.Angle(relativePos, vel);
        Debug.Log("relative Angle is " + relativeAngle);
        if (relativeAngle > 120 && vel.magnitude > motionThreshold)
        {
            Debug.Log("Telling to Dig");
            StartCoroutine(myDirt.DigHole());
        }
        if (relativeAngle < 40 && vel.magnitude > motionThreshold)
        {
            Debug.Log("Telling to Cover");
            StartCoroutine(myDirt.CoverHole());
        }
    }
}