using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWhenHit : MonoBehaviour {

    private const float SOUND_SCALE = 0.25f;

    private AudioSource audioSrc;
    private float originalVolume;

    private void Start()
    {
        audioSrc = gameObject.GetComponent<AudioSource>();
        originalVolume = audioSrc.volume;
    }

    private void OnCollisionEnter(Collision collision)
    {
        audioSrc.volume = collision.relativeVelocity.magnitude * SOUND_SCALE * originalVolume;
        print(gameObject + " BONK " + audioSrc.volume);
        audioSrc.Play();
    }
}
