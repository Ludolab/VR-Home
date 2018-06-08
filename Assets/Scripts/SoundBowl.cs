using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBowl : Bowl {

    private AudioSource audioSrc;

    private void Start()
    {
        audioSrc = gameObject.GetComponent<AudioSource>();
    }

    protected override float GetValue()
    {
        return audioSrc.volume;
    }

    protected override void SetValue(float newValue)
    {
        audioSrc.volume = newValue;
    }
}
