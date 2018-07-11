using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : MonoBehaviour
{

	public GameObject particleObj;

	private AudioSource audioSrc;
	private ParticleSystem ps;
	private bool particlesActive = false;

	private void Start()
	{
		audioSrc = particleObj.GetComponent<AudioSource>();
		ps = particleObj.GetComponent<ParticleSystem>();
		ps.Stop();
	}

	private void Update()
	{
		if (IsTilted() && !particlesActive)
		{
			ps.Play();
			audioSrc.Play();
			particlesActive = true;
		}

		if (!IsTilted() && particlesActive)
		{
			ps.Stop();
			audioSrc.Stop();
			particlesActive = false;
		}
	}

	private bool IsTilted()
	{
		return InRange(transform.localEulerAngles.z);
	}

	private bool InRange(float angle)
	{
		return angle > 45 && angle < 180;
	}
}
