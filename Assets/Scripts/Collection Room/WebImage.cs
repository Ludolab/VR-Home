using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebImage : MonoBehaviour
{

    public string imageUrl;

    private Renderer rend;
    private Vector2 originalScale;
    
    private void Start()
    {
        originalScale = transform.localScale;
        rend = GetComponent<Renderer>();
        StartCoroutine(DownloadImage());
    }

    private IEnumerator DownloadImage()
    {
        using (WWW www = new WWW(imageUrl))
        {
            yield return www;

            Texture2D texture = www.texture;
            SetTexture(texture);
        }
    }

    private void SetTexture(Texture2D texture)
    {
        float width = originalScale.x;
        float height = originalScale.y;

        float aspect = (float)texture.width / texture.height;
        print(aspect);
        if (aspect > 1)
        {
            height /= aspect;
        }
        else
        {
            width *= aspect;
        }

        transform.localScale = new Vector3(width, height, 1);
        rend.material.mainTexture = texture;
    }

}
