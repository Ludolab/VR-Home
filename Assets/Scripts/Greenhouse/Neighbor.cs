using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighbor : MonoBehaviour
{

	public NeighborInfo info;
	public GameObject letterPrefab;
	public string[] todaysGift = new string[0];

	public GameObject GenerateLetter(string text1, string text2, Vector3 position)
	{
		GameObject letterObj = Instantiate(letterPrefab, position, Quaternion.identity);
		Letter letter = letterObj.GetComponent<Letter>();
		letter.SetContents(text1, text2, info.font, info.fontMaterial);
		return letterObj;
	}

	public void StartDay(int day)
	{
		if (day < info.letters.Length)
		{
			NeighborInfo.LetterInfo l = info.letters[day];
			if (l.exists)
			{
				print("[Day " + day + "] Requires gift: " + l.dependsOnGift + ", gift: " + todaysGift);
				if (!l.dependsOnGift || todaysGift.Length > 0) //TODO: type of gift
				{
					GenerateLetter(l.text1, l.text2, new Vector3(0, 1f, 0)); //TODO: position
				}
			}
		}

		if (day == info.dayLabelUnlocked)
		{
			TimeManager.instance.AddOutboxLabel(info);
		}

		todaysGift = new string[0];
	}

	public void GiveGift(string[] gift)
	{
		todaysGift = gift;
	}
}
