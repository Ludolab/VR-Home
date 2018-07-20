using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beetle : MonoBehaviour
{

	public float flickThreshold;
	public float flickMultiplier;

	public GameObject particlePrefab;
	public AudioClip squishSound;
	public Color squishColor;

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

	public void Squish()
	{
		SpawnParticles();
        if(plotIn != null) plotIn.RemoveFromBeetles(gameObject);
		Destroy(gameObject);
	}

	private void SpawnParticles()
	{
		GameObject particles = Instantiate(particlePrefab, transform.position, Quaternion.identity);
		ParticleSystem ps = particles.GetComponent<ParticleSystem>();
		ParticleSystem.MainModule main = ps.main;
		main.startColor = squishColor;
		AudioSource aud = particles.GetComponent<AudioSource>();
		aud.clip = squishSound;
	}

    public void setPlot(Plot plot) {
        plotIn = plot;
    }
}
