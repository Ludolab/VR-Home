using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtDigger : MonoBehaviour
{

    public Dirt myDirt;
    public float motionThreshold;

    private void OnCollisionStay(Collision collision)
    {
        bool isLeapHand = collision.gameObject.name.StartsWith("Contact");

        if (isLeapHand)
        {
            Vector3 relativePos = this.gameObject.transform.position - collision.gameObject.transform.position;
            Vector3 vel = collision.gameObject.GetComponent<Rigidbody>().velocity;
            float relativeAngle = Vector3.Angle(relativePos, vel);
            if (relativeAngle > 150 && vel.magnitude > motionThreshold)
            {
                StartCoroutine(myDirt.DigHole());
            }
            if (relativeAngle < 30 && vel.magnitude > motionThreshold)
            {
                StartCoroutine(myDirt.CoverHole());
            }
        }
    }
}