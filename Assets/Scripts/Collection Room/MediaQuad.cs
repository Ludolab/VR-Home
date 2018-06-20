using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediaQuad : MonoBehaviour
{
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
}
