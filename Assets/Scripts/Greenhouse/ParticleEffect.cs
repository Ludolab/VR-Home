﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{

    private AudioSource audioSrc;

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        audioSrc.Play();
    }

    private void Update()
    {
        if (!audioSrc.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
