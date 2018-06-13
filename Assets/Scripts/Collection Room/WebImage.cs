using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using UnityEngine;

public class WebImage : MonoBehaviour
{

    private const string IMAGE_URL = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/81/2012_Suedchinesischer_Tiger.JPG/220px-2012_Suedchinesischer_Tiger.JPG";

    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        StartCoroutine(DownloadImage());
    }

    private IEnumerator DownloadImage()
    {
        using (WWW www = new WWW(IMAGE_URL))
        {
            yield return www;

            rend.material.mainTexture = www.texture;
        }
    }
    
}
