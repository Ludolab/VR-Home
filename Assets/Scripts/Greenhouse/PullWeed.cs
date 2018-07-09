﻿using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullWeed : MonoBehaviour
{

	public AudioClip popOutClip;

	public float pullDistance;

	private InteractionBehaviour ib;
	private Rigidbody rb;
	private AudioSource audioSrc;

	private bool grasped = false;
	private bool pulled = false;
	private Vector3 basePosition;
	private Vector3 startScale;

	private void Start()
	{
		ib = GetComponent<InteractionBehaviour>();
		rb = GetComponent<Rigidbody>();
		audioSrc = GetComponent<AudioSource>();
		startScale = transform.localScale;
	}

	public void OnGrasp()
	{
		grasped = true;
		basePosition = GetHandPosition();
	}

	private Vector3 GetHandPosition()
	{
		return ib.graspingController.transform.position;
	}

	public void HoldGrasp()
	{
		//TODO: could play some sort of stretchy sound here?

		Vector3 newPosition = GetHandPosition();

		//TODO: stretch/turn around anchor point in roots
		Vector3 diff = newPosition - basePosition;
		float dist = diff.magnitude;

		float yScale = startScale.y + dist;
		transform.localScale = new Vector3(startScale.x, yScale, startScale.z);

		/*Vector3 from = basePosition - transform.position;
		Vector3 to = newPosition - transform.position;
		transform.rotation = Quaternion.FromToRotation(from, to);*/

		if (dist < pullDistance)
		{
			transform.localScale = startScale;
			PullOut();
		}
	}

	private void PullOut()
	{
		audioSrc.PlayOneShot(popOutClip);
		rb.isKinematic = false;
		rb.useGravity = true;
		ib.moveObjectWhenGrasped = true;
		//TODO: snap into hand?
	}

	public void OnUngrasp()
	{
		grasped = false;
	}

	public void OnContact()
	{
		if (!grasped && !pulled)
		{
			//TODO: wobble
		}
	}
}
