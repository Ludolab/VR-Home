using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Record : MonoBehaviour
{
    public GameObject colorObj;

    private AudioClip ac;
    private Renderer rend;

    private void Awake()
    {
        rend = colorObj.GetComponent<Renderer>();
    }

    public void SetAudio(AudioClip audioClip)
    {
        ac = audioClip;
        rend.material.color = AudioToColor(audioClip);
    }

    public AudioClip GetAudio()
    {
        return ac;
    }

    public Color GetColor()
    {
        return rend.material.color;
    }

    private static Color AudioToColor(AudioClip audioClip)
    {
        Random.InitState(audioClip.GetHashCode());
        return Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1.0f);
    }
}
