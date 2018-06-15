using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoLoader : MonoBehaviour
{

    private const float HEIGHT_DIFF = 0.2f;

    public GameObject photoPrefab;

	private void Start()
    {
        Texture2D[] textures = Resources.LoadAll<Texture2D>("Photos");
        float height = transform.position.y;
        foreach (Texture2D tex in textures)
        {
            print("Generating " + tex.name);
            GameObject photo = Instantiate(photoPrefab);
            photo.GetComponent<ImageQuad>().SetTexture(tex);
            photo.transform.position = new Vector3(transform.position.x, height, transform.position.z);
            height += HEIGHT_DIFF;
        }
	}
}
