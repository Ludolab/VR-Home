﻿using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullWeed : MonoBehaviour
{

	public AudioClip popOutClip;

	public float pullDistance;

	public GameObject dragObj;

	private InteractionBehaviour ib;
	private Rigidbody rb;
	private Collider col;
	private AudioSource audioSrc;
	private Rigidbody dragRB;

	private bool grasped = false;
	private bool pulled = false;
	private Vector3 basePosition;
	private Vector3 offset;
	private Vector3 startScale;

	private void Start()
	{
		ib = GetComponent<InteractionBehaviour>();
		rb = GetComponent<Rigidbody>();
		dragRB = dragObj.GetComponent<Rigidbody>();
		col = GetComponent<Collider>();
		audioSrc = GetComponent<AudioSource>();
		startScale = transform.localScale;
	}

	public void OnGrasp()
	{
		grasped = true;
		basePosition = GetDragPosition();
		offset = transform.position - basePosition;
		print("GRASP");
		//TODO: could play some sort of looping stretchy sound here?
	}

	private Vector3 GetDragPosition()
	{
		return dragRB.position;
	}

	private void FixedUpdate()
	{
		if (grasped)
		{
			Vector3 newPosition = GetDragPosition();

			//TODO: stretch/turn around anchor point in roots
			Vector3 diff = newPosition - basePosition;
			float dist = diff.magnitude;
			//print("base: " + basePosition + ", new: " + newPosition + ", dist: " + dist);

			float yScale = startScale.y + dist;
			transform.localScale = new Vector3(startScale.x, yScale, startScale.z);

			transform.position = (newPosition + basePosition) / 2 + offset;

			/*Vector3 from = basePosition - transform.position;
			Vector3 to = newPosition - transform.position;
			transform.rotation = Quaternion.FromToRotation(from, to);*/

			//TODO: adjust pitch of stretchy sound?

			if (dist > pullDistance)
			{
				transform.localScale = startScale;
				print("POP");
				PullOut();
			}
		}
	}

	private void PullOut()
	{
		audioSrc.PlayOneShot(popOutClip);
		dragObj.SetActive(false);
		col.enabled = true;
		rb.isKinematic = false;
		rb.useGravity = true;
		ib.enabled = true;
		//TODO: grasp, snap into hand
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
