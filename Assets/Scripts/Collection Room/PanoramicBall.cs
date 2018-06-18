﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanoramicBall : MonoBehaviour {

    public static Texture viewedImage;

    public Texture thisImage;

    Material myMaterial;

	private void Start()
	{
        myMaterial = GetComponent<Renderer>().material;
        myMaterial.mainTexture = thisImage;
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Camera (eye)")
        {
            viewedImage = thisImage;
            SceneManager.LoadScene("View 360");
        }
    }
}
