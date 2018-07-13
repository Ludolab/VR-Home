using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtDigger : MonoBehaviour
{

    public Dirt myDirt;
    public float motionThreshold;

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Collision with Dirt");
        bool isLeapHand = collision.gameObject.name.StartsWith("Contact");

        if (isLeapHand)
        {
            Debug.Log("Collider is Hand");
            Vector3 relativePos = this.gameObject.transform.position - collision.gameObject.transform.position;
            Vector3 vel = collision.gameObject.GetComponent<Rigidbody>().velocity;
            float relativeAngle = Vector3.Angle(relativePos, vel);
            Debug.Log("relative Angle is " + relativeAngle);
            if (relativeAngle > 150 && vel.magnitude > motionThreshold)
            {
                Debug.Log("Telling to Dig");
                StartCoroutine(myDirt.DigHole());
            }
            if (relativeAngle < 30 && vel.magnitude > motionThreshold)
            {
                Debug.Log("Telling to Cover");
                StartCoroutine(myDirt.CoverHole());
            }
        }
    }
}