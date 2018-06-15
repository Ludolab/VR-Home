using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageQuad : MonoBehaviour
{

    private Renderer rend;
    private Vector2 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
        rend = GetComponent<Renderer>();
    }

    public void SetTexture(Texture2D texture)
    {
        float width = originalScale.x;
        float height = originalScale.y;

        float aspect = (float)texture.width / texture.height;
        if (aspect > 1)
        {
            height /= aspect;
        }
        else
        {
            width *= aspect;
        }

        transform.localScale = new Vector3(width, height, transform.localScale.z);
        rend.material.mainTexture = texture;
    }

}
