using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beetle : MonoBehaviour
{

	public float flickThreshold;
	public float flickMultiplier;

	public GameObject particlePrefab;

	private Rigidbody rb;
	private InteractionBehaviour ib;

	private bool isFlicked;

    private Plot plotIn;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		ib = GetComponent<InteractionBehaviour>();
		isFlicked = false;
	}

	private void OnCollisionStay(Collision collision)
	{
		bool isLeapHand = collision.gameObject.name.StartsWith("Contact"); //may not be fully accurate but doesn't really matter
		if (isFlicked && !isLeapHand)
		{
			Squish();
		}
	}

	public void ContactStay()
	{
		InteractionController closestController = ib.closestHoveringController;
		if (!isFlicked)
		{
			Vector3 vel = closestController.velocity;
			if (vel.magnitude > flickThreshold)
			{
				isFlicked = true;
				Flick(vel);
			}
		}
	}

	private void Flick(Vector3 vel)
	{
		rb.isKinematic = false;
		rb.useGravity = true;
		rb.AddForce(vel * flickMultiplier, ForceMode.Impulse);
		if (plotIn != null) plotIn.RemoveFromBeetles(gameObject); //do this here too in case it doesn't land on anything
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
