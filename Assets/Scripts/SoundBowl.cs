using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBowl : Bowl {

    private AudioSource[] audioSrcs;
    private float globalVolume;
    private float[] maxVolumes;

    private void Start()
    {
        audioSrcs = GetComponents<AudioSource>();
        int len = audioSrcs.Length;
        maxVolumes = new float[len];
        for (int i = 0; i < len; i++)
        {
            maxVolumes[i] = audioSrcs[i].volume;
            audioSrcs[i].volume = 0;
        }
    }

    protected override float GetValue()
    {
        return globalVolume;
    }

    protected override void SetValue(float newValue)
    {
        globalVolume = newValue;
        int len = audioSrcs.Length;
        for (int i = 0; i < len; i++)
        {
            audioSrcs[i].volume = maxVolumes[i] * globalVolume;
        }
    }
}
