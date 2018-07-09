﻿using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Envelope : MonoBehaviour
{

	public GameObject textObj1;
	public GameObject textObj2;
	public GameObject paperObj;
	public GameObject hingeObj;

	private int numJoints;
	private InteractionBehaviour ib;

	private void Start()
	{
		numJoints = hingeObj.GetComponents<Joint>().Length;
		ib = hingeObj.GetComponent<InteractionBehaviour>();
		TimeManager.instance.AddGarbage(gameObject);
	}

	public void JointBroke()
	{
		StartCoroutine(RefreshLocked());
		numJoints--;
		if (numJoints == 0)
		{
			paperObj.transform.parent = null;
			paperObj.SetActive(true);
			TimeManager.instance.AddGarbage(paperObj);
		}
	}

	private IEnumerator RefreshLocked()
	{
		yield return null;
		ib.RefreshPositionLockedState();
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
