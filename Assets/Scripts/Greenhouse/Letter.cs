﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Letter : MonoBehaviour
{

	public GameObject textObj1;
	public GameObject textObj2;
	public GameObject paperObj;
	public GameObject hingeObj;

	private int numJoints;

	private void Start()
	{
		numJoints = hingeObj.GetComponents<Joint>().Length;
	}

	public void JointBroke()
	{
		numJoints--;
		if (numJoints == 0)
		{
			paperObj.SetActive(true);
		}
	}

	public void SetContents(string text1, string text2, Font font, Material fontMaterial)
	{
		TextMesh textMesh1 = textObj1.GetComponent<TextMesh>();
		TextMesh textMesh2 = textObj2.GetComponent<TextMesh>();
		Renderer textRend1 = textObj1.GetComponent<Renderer>();
		Renderer textRend2 = textObj2.GetComponent<Renderer>();

		textMesh1.text = text1;
		textMesh2.text = text2;
		textMesh1.font = font;
		textRend1.material = fontMaterial;
		textMesh2.font = font;
		textRend2.material = fontMaterial;
	}
}
