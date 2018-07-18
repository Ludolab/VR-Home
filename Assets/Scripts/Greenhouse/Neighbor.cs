using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighbor : MonoBehaviour
{

	public NeighborInfo info;
	public Outbox outbox;
	public GameObject letterPrefab;

	private DialogueParser parser;
	private string[] todaysGift = new string[0];

	private void Awake()
	{
		parser = GetComponent<DialogueParser>();
		parser.SetCanvas(info.dialogueCanvas);
	}

	public GameObject GenerateLetter(string text1, string text2)
	{
		Vector3 position = outbox.transform.position; //TODO: offset?
		GameObject letterObj = Instantiate(letterPrefab, position, Quaternion.identity);
		Letter letter = letterObj.GetComponent<Letter>();
		letter.SetContents(text1, text2, info.font, info.fontMaterial, info.paperTexture, info.textColor);
		return letterObj;
	}

	public void StartDay(int day)
	{
		outbox.SetLabel(info);
		parser.NextDay(this);

		foreach (NeighborInfo.LetterInfo l in info.letters)
		{
			if (l.day == day)
			{
				//print("[Day " + day + "] Requires gift: " + l.dependsOnGift + ", gift: " + todaysGift);
				if (CanSpawn(l))
				{
					GenerateLetter(l.text1, l.text2);
				}
			}
		}

		todaysGift = new string[0];
	}

	public void GiveGift(string[] gift)
	{
		todaysGift = gift;
	}

	private bool CanSpawn(NeighborInfo.LetterInfo letter)
	{
		if (letter.spawnOption == NeighborInfo.SpawnOption.Always)
		{
			return true;
		}

		if (letter.spawnOption == NeighborInfo.SpawnOption.RequireGift && todaysGift.Length > 0)
		{
			return true;
		}

		if (letter.spawnOption == NeighborInfo.SpawnOption.RequireNoGift && todaysGift.Length == 0)
		{
			return true;
		}

		return false;
	}
}
