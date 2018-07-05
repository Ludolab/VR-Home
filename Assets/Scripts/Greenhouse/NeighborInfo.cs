﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NeighborInfo", menuName = "Neighbor")]
public class NeighborInfo : ScriptableObject
{
	
	[Serializable]
	public struct LetterInfo
	{
		public bool exists;

		[TextArea(3, 10)]
		public string text1;

		[TextArea(3, 10)]
		public string text2;
	}

	public Material fontMaterial;
	public Font font;
	//TODO: paper texture

	public LetterInfo[] letters;
	//TODO: gifts
	//TODO: conditional somehow on gifts from player

}
