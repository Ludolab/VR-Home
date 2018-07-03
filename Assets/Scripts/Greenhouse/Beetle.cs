using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beetle : MonoBehaviour
{

	public float flickThreshold;
	public float flickMultiplier;

	private Rigidbody rb;

	private bool isFlicked;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		isFlicked = false;
	}

	private void OnCollisionStay(Collision collision)
	{
		bool isLeapHand = collision.gameObject.name.StartsWith("Contact");

		if (!isFlicked && isLeapHand)
		{
			Vector3 vel = collision.gameObject.GetComponent<Rigidbody>().velocity;
			if (vel.magnitude > flickThreshold)
			{
				isFlicked = true;
				Flick(vel);
			}
		}
		if (isFlicked && !isLeapHand)
		{
			Squish();
		}
	}

	private void Flick(Vector3 vel)
	{
		rb.isKinematic = false;
		rb.useGravity = true;
		rb.AddForce(vel * flickMultiplier, ForceMode.Impulse);
	}

	private void Squish()
	{
		//TODO: sound, particles
		Destroy(gameObject);
	}
}
