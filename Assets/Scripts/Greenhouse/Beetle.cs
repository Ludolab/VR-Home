﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beetle : MonoBehaviour
{

	public float flickThreshold;
	public float flickMultiplier;

	public GameObject particlePrefab;

	private Rigidbody rb;

	private bool isFlicked;

    private Plot plotIn;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		isFlicked = false;
	}

	private void OnCollisionStay(Collision collision)
	{
		bool isLeapHand = collision.gameObject.name.StartsWith("Contact");
		print("collision stay with " + collision.gameObject.name + ", ishand = " + isLeapHand);
		if (!isFlicked && isLeapHand)
		{
			Vector3 vel = collision.gameObject.GetComponent<Rigidbody>().velocity;
			print("speed: " + vel.magnitude);
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

	[ContextMenu("Squish")]
	public void Squish()
	{
		SpawnParticles();
        if(plotIn != null) plotIn.RemoveFromBeetles(gameObject);
		Destroy(gameObject);
	}

	private void SpawnParticles()
	{
		Instantiate(particlePrefab, transform.position, Quaternion.identity);
	}

    public void setPlot(Plot plot) {
        plotIn = plot;
    }
}
