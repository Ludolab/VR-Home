using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Neighbor : MonoBehaviour
{

	public NeighborInfo info;
	public Outbox outbox;
	public GameObject pagePrefab;
	public GameObject seedPrefab;

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
		if (info.ownsOutbox)
		{
			outbox.SetOwner(this);
		}
	}

	public GameObject GeneratePage(string text)
	{
		Vector3 anchorPos = outbox.paperTransform.position;
		Vector3 position = anchorPos + pagePrefab.transform.localScale.y / 2 * Vector3.down; //move half down to anchor at hook
		GameObject pageObj = Instantiate(pagePrefab, position, pagePrefab.transform.rotation);
		Page page = pageObj.GetComponent<Page>();
		page.SetContents(text, info.font, info.fontMaterial, info.paperTexture, info.fontSize);
		return pageObj;
	}

	public GameObject GenerateSeed(string seedName)
	{
		Vector3 position = outbox.transform.position; //TODO: offset?
		GameObject seedObj = Instantiate(seedPrefab, position, seedPrefab.transform.rotation);
		Starter starter = seedObj.GetComponent<Starter>();
		starter.plantName = seedName;
        starter.RefreshLabel();
        TimeManager.instance.AddStarter(starter);
		return seedObj;
	}

	public void GenerateSeeds(string[] seedNames)
	{
		foreach (string seedName in seedNames)
		{
			GenerateSeed(seedName);
		}
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
					GeneratePage(l.text);
					GenerateSeeds(l.gifts);
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

		if (letter.spawnOption == NeighborInfo.SpawnOption.RequireGiftType)
		{
			return todaysGift.Contains(letter.requiredGiftType);
		}

		return false;
	}
}
