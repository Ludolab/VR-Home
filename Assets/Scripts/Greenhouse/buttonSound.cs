using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonSound : MonoBehaviour {

  AudioSource button;
  public AudioClip pressSoundClip;
	public AudioClip unpressSoundClip;

	// Use this for initialization
	void Start () {
		button = GetComponent<AudioSource>();
	}

	public void pressSound() {
		button.clip = pressSoundClip;
		button.Play();
	}
	public void unpressSound() {
		button.clip = unpressSoundClip;
		button.Play();
	}
}
