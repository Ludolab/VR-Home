using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NeighborInfo", menuName = "Neighbor")]
public class NeighborInfo : ScriptableObject
{
	
	[Serializable]
	public struct LetterInfo
	{
		[TextArea(3, 10)]
		public string text1;

		[TextArea(3, 10)]
		public string text2;

		public bool dependsOnGift;
		//TODO: type of gift required

		public bool Exists()
		{
			return (text1 != null && text1 != "") || (text2 != null && text2 != "");
		}
	}

	public Material fontMaterial;
	public Font font;
	//TODO: paper texture

	public LetterInfo[] letters;
	//TODO: gifts with letters

	public DialogueCanvasType dialogueCanvas;
	
	public int dayLabelUnlocked; //index of day on which this person's label becomes selectable in outbox
}
