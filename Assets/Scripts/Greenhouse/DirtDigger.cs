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
        Vector3 myPos = this.gameObject.transform.position;
        Vector2 myPosFlat = new Vector2(myPos.x, myPos.z);
        Vector2 relativePos = myPosFlat - new Vector2(myController.transform.position.x, myController.transform.position.z);
        Vector3 vel = myController.velocity;
        Vector2 velFlat = new Vector2(vel.x, vel.z);
        float relativeAngle = Vector2.Angle(relativePos, velFlat);
        //Debug.Log("relative Angle is " + relativeAngle);
        if (relativeAngle > 130 && vel.magnitude > motionThreshold)
        {
            //Debug.Log("Telling to Dig");
            StartCoroutine(myDirt.DigHole());
        }
        if (relativeAngle < 50 && vel.magnitude > motionThreshold)
        {
            //Debug.Log("Telling to Cover");
            StartCoroutine(myDirt.CoverHole());
        }
    }
}