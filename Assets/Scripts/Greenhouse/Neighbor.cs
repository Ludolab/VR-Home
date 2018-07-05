using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighbor : MonoBehaviour
{

	public Material fontMaterial;
	public Font font;

	public GameObject letterPrefab;

	public GameObject GenerateLetter(string text1, string text2)
	{
		GameObject letterObj = Instantiate(letterPrefab);
		Letter letter = letterObj.GetComponent<Letter>();
		letter.SetContents(text1, text2, font, fontMaterial);
		return letterObj;
	}

}
