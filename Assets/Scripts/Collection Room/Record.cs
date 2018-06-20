using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Record : MonoBehaviour
{
    private AudioClip ac;
    private Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    public void SetAudio(AudioClip audioClip)
    {
        ac = audioClip;
        rend.material.color = GetColor(audioClip);
    }

    public AudioClip GetAudio()
    {
        return ac;
    }

    private static Color GetColor(AudioClip audioClip)
    {
        Random.InitState(audioClip.GetHashCode());
        return Random.ColorHSV(0f, 1f, 1f, 1f, 0.1f, 0.6f);
    }
}
