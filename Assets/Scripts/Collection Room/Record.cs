﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Record : MonoBehaviour
{
    public GameObject colorObj;
    public GameObject textObj;

    private AudioClip ac;
    private Renderer rend;
    private TextMesh text3d;

    private void Awake()
    {
        rend = colorObj.GetComponent<Renderer>();
        text3d = textObj.GetComponent<TextMesh>();
    }

    public void SetAudio(AudioClip audioClip)
    {
        ac = audioClip;
        rend.material.color = AudioToColor(audioClip);
        text3d.text = ac.name;
    }

    public AudioClip GetAudio()
    {
        return ac;
    }

    public Color GetColor()
    {
        return rend.material.color;
    }

    public string GetText()
    {
        return text3d.text;
    }

    private static Color AudioToColor(AudioClip audioClip)
    {
        Random.InitState(audioClip.GetHashCode());
        return Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1.0f);
    }
}
