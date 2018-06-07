using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBowl : Bowl {

    private const float VOLUME_TRANSITION_TIME = 1.0f;

    public int maxObjects;

    private AudioSource audioSrc;

    private void Start()
    {
        audioSrc = gameObject.GetComponent<AudioSource>();
    }

    protected override void Refresh()
    {
        StartCoroutine(TransitionToVolume(((float)numObjects) / maxObjects));
    }

    private IEnumerator TransitionToVolume(float newVolume)
    {
        float oldVolume = audioSrc.volume;
        for (float t = 0; t < VOLUME_TRANSITION_TIME; t += Time.deltaTime)
        {
            audioSrc.volume = Mathf.Lerp(oldVolume, newVolume, t / VOLUME_TRANSITION_TIME);
            yield return new WaitForEndOfFrame();
        }
        audioSrc.volume = newVolume;
    }
}
