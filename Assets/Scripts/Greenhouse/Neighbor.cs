using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighbor : MonoBehaviour
{

	public NeighborInfo info;
	public Outbox outbox;
	public GameObject pagePrefab;

	private DialogueParser parser;
	private string[] todaysGift = new string[0];

	private void Awake()
	{
		parser = GetComponent<DialogueParser>();
		parser.SetCanvas(info.dialogueCanvas);
	}

	private void Start()
	{
		info.fontMaterial.color = info.textColor;
		outbox.SetLabel(info);
	}

	public GameObject GeneratePage(string text)
	{
		Vector3 position = outbox.transform.position; //TODO: offset?
		GameObject pageObj = Instantiate(pagePrefab, position, pagePrefab.transform.rotation);
		Page page = pageObj.GetComponent<Page>();
		page.SetContents(text, info.font, info.fontMaterial, info.paperTexture);
		return pageObj;
	}

	public void StartDay(int day)
	{
		parser.NextDay(this);

		foreach (NeighborInfo.LetterInfo l in info.letters)
		{
			if (l.day == day)
			{
				//print("[Day " + day + "] Requires gift: " + l.dependsOnGift + ", gift: " + todaysGift);
				if (CanSpawn(l))
				{
					GeneratePage(l.text1);
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
