using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpLeap : MonoBehaviour
{

	public bool outlineEnabled = true; //uncheck to disable outlines completely (without having to remove outline material)

	public Color hoverColor;
	public Color heldColor;

	private Renderer rend;
	private Material outlineMat;

	private Material[] noOutline;
	private Material[] withOutline;

	private bool hovered;
	private bool held;

	private void Start()
	{
		rend = GetComponent<Renderer>();
		if (rend.materials.Length < 2)
		{
			throw new Exception(gameObject + " must have an outline shader and a regular shader");
		}

		outlineMat = rend.materials[0];
		withOutline = rend.materials;
		noOutline = new Material[] {
			rend.materials[1]
		};

		SetOutline(false);
	}

	public void OnHover()
	{
		hovered = true;

		if (!held)
		{
			SetOutline(true);
			SetColor(hoverColor);
		}
	}

	public void OnUnhover()
	{
		hovered = false;

		if (!held)
		{
			SetOutline(false);
		}
	}

	public void OnGrasp()
	{
		held = true;
		SetOutline(true);
		SetColor(heldColor);
	}

	public void OnUngrasp()
	{
		held = false;

		if (hovered)
		{
			SetOutline(true);
			SetColor(hoverColor);
		}
		else
		{
			SetOutline(false);
		}
	}

	private void SetOutline(bool outlined)
	{
		rend.materials = (outlineEnabled && outlined) ? withOutline : noOutline;
	}

	private void SetColor(Color color)
	{
		outlineMat.SetColor("_OutlineColor", color);
	}
}
