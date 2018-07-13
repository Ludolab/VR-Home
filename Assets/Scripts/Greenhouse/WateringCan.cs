using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : MonoBehaviour
{

	public GameObject particleObj;
	public float soundFadeTime;

	private AudioSource audioSrc;
	private ParticleSystem ps;
	private bool particlesActive = false;
	private float maxVolume;

	private void Start()
	{
		audioSrc = GetComponent<AudioSource>();
		maxVolume = audioSrc.volume;
		audioSrc.volume = 0;
		ps = particleObj.GetComponent<ParticleSystem>();
		ps.Stop();
	}

	private void Update()
	{
		if (IsTilted() && !particlesActive)
		{
			ps.Play();
			StopCoroutine(StopSound());
			StartCoroutine(PlaySound());
			particlesActive = true;
		}

		if (!IsTilted() && particlesActive)
		{
			ps.Stop();
			StopCoroutine(PlaySound());
			StartCoroutine(StopSound());
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

	private IEnumerator PlaySound()
	{
		audioSrc.Play();
		float startVolume = audioSrc.volume;
		for (float t = 0; t < soundFadeTime; t += Time.deltaTime)
		{
			audioSrc.volume = Mathf.Lerp(startVolume, maxVolume, t);
			yield return new WaitForEndOfFrame();
		}
		audioSrc.volume = maxVolume;
	}

	private IEnumerator StopSound()
	{
		float startVolume = audioSrc.volume;
		for (float t = 0; t < soundFadeTime; t += Time.deltaTime)
		{
			audioSrc.volume = Mathf.Lerp(startVolume, 0, t);
			yield return new WaitForEndOfFrame();
		}
		audioSrc.volume = 0;
		audioSrc.Stop();
	}
}
