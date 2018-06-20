using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageQuad : MediaQuad
{

    public GameObject rendObject;

    private Renderer rend;

    protected override void Awake()
    {
        base.Awake();

        if (rendObject == null)
        {
            rendObject = gameObject;
        }
        rend = rendObject.GetComponent<Renderer>();
    }

    public void SetTexture(Texture2D texture)
    {
        Scale(texture.width, texture.height);
        rend.material.mainTexture = texture;
    }

}
