using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpLeap : MonoBehaviour
{
	public Color hoverColor;
	public Color heldColor;

	private Renderer rend;
	private Material outlineMat;

	private Material[] noOutline;
	private Material[] withOutline;

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
		SetOutline(true);
		SetColor(hoverColor);
	}

	public void OnUnhover()
	{
		SetOutline(false);
	}

	public void OnGrasp()
	{
		SetOutline(true);
		SetColor(heldColor);
	}

	public void OnUngrasp()
	{
		SetOutline(true);
		SetColor(hoverColor);
	}

	private void SetOutline(bool enabled)
	{
		rend.materials = enabled ? withOutline : noOutline;
	}

	private void SetColor(Color color)
	{
		outlineMat.SetColor("_OutlineColor", color);
	}
}
