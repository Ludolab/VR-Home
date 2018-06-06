using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBowl : Bowl {

    public int maxObjects;

    private AudioSource audioSrc;

    private void Start()
    {
        audioSrc = gameObject.GetComponent<AudioSource>();
    }

    protected override void Refresh()
    {
        audioSrc.volume = ((float)numObjects) / maxObjects;
    }
}
