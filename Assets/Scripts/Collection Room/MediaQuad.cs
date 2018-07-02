using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediaQuad : MonoBehaviour
{
    //Hovering triggers audio
    public AudioClip hoverClip;
    public float volLowRange = .5f;
    public float volHighRange = 1.0f;

    private Vector2 originalScale;

    protected virtual void Awake()
    {
        originalScale = transform.localScale;
    }

    protected void Scale(float mediaWidth, float mediaHeight)
    {
        float width = originalScale.x;
        float height = originalScale.y;

        float aspect = mediaWidth / mediaHeight;
        if (aspect > 1)
        {
            height /= aspect;
        }
        else
        {
            width *= aspect;
        }

        transform.localScale = new Vector3(width, height, transform.localScale.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        //play sound
        if (other.gameObject.name.StartsWith("Controller"))
        {
            print("play sound");
            GetComponent<AudioSource>().PlayOneShot(hoverClip);
        }
    }

}
