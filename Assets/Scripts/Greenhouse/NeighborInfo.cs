using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NeighborInfo", menuName = "Neighbor")]
public class NeighborInfo : ScriptableObject
{
	
	public enum SpawnOption
	{
		Always,
		RequireGift,
		RequireNoGift
	}

	[Serializable]
	public struct LetterInfo
	{
		[TextArea(3, 15)]
		public string text1;

		public string[] gifts;

		public int day;

		public SpawnOption spawnOption;
		//TODO: type of gift required?
	}

	public Material fontMaterial;
	public Font font;
	public Texture paperTexture;
	public Color textColor;
	public int fontSize;
	public bool ownsOutbox = true;
	public string labelOverride;

	public LetterInfo[] letters;

	public DialogueCanvasType dialogueCanvas;
}
