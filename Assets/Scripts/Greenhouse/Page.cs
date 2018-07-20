using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Page : MonoBehaviour
{

	public GameObject textObj;
	public GameObject paperObj;
	public GameObject paperBackObj;

	private ConfigurableJoint joint;
	private InteractionBehaviour ib;

	private void Awake()
	{
		joint = GetComponent<ConfigurableJoint>();
		ib = GetComponent<InteractionBehaviour>();
	}

	private void Start()
	{
		TimeManager.instance.AddGarbage(gameObject);
	}

	public void SetContents(string text, Font font, Material fontMaterial, Texture paperTexture, int fontSize)
	{
		TextMesh textMesh = textObj.GetComponent<TextMesh>();
		Renderer textRend = textObj.GetComponent<Renderer>();
		Renderer paperRend = paperObj.GetComponent<Renderer>();
		Renderer paperBackRend = paperBackObj.GetComponent<Renderer>();

		textMesh.text = text;
		textMesh.font = font;
		textMesh.fontSize = fontSize;
		textRend.material = fontMaterial;

		paperRend.material.mainTexture = paperTexture;
		paperBackRend.material.mainTexture = paperTexture;
	}

	public void Grab()
	{
		Destroy(joint);
		StartCoroutine(RefreshLocked());
	}

	private IEnumerator RefreshLocked()
	{
		yield return null;
		ib.RefreshPositionLockedState();
	}

}
