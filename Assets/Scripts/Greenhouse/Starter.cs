using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starter : MonoBehaviour {

    public string plantName;
    public GameObject myTape;

	//TODO: render the seed of the plant at the top of the starter.

	private void Start()
	{
        Material tapeMaterial = myTape.GetComponent<Renderer>().material;
        tapeMaterial.mainTexture = (Texture)Resources.Load("Textures/Starter_Labels/" + plantName);
	}
}
