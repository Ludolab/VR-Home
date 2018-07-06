﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outbox : MonoBehaviour
{

	public GameObject labelPrefab;
	public Transform labelTransform;

	private List<GameObject> labels = new List<GameObject>();
	private int labelIndex = 0;
	private GameObject activeLabel;

	private List<Fruit> fruits = new List<Fruit>();

	public void AddLabel(NeighborInfo info)
	{
		GameObject label = Instantiate(labelPrefab, labelTransform);
		GameObject labelText = label.transform.Find("text").gameObject;
		TextMesh labelMesh = labelText.GetComponent<TextMesh>();
		Renderer labelRend = labelText.GetComponent<Renderer>();

		labelMesh.text = info.name;
		labelMesh.font = info.font;
		labelRend.material = info.fontMaterial;

		label.SetActive(false);
		labels.Add(label);

		UpdateLabel(); //in case this is the first
	}

	[ContextMenu("Next Label")]
	public void NextLabel()
	{
		ChangeLabel(1);
	}

	[ContextMenu("Prev Label")]
	public void PrevLabel()
	{
		ChangeLabel(-1);
	}

	private void ChangeLabel(int addition)
	{
		labelIndex = (labelIndex + addition) % labels.Count;
		if (labelIndex < 0)
		{
			labelIndex += labels.Count;
		}
		UpdateLabel();
	}

	private void UpdateLabel()
	{
		if (activeLabel != null)
		{
			activeLabel.SetActive(false);
		}
		if (labelIndex < labels.Count)
		{
			activeLabel = labels[labelIndex];
			activeLabel.SetActive(true);
		}
	}

	public void AddFruit(Fruit f)
	{
		fruits.Add(f);
	}

	public void RemoveFruit(Fruit f)
	{
		fruits.Remove(f);
	}

	public string[] ClearFruit()
	{
		List<string> fruitNames = new List<string>();
		foreach (Fruit f in fruits)
		{
			fruitNames.Add(f.GetName()); 
			Destroy(f.gameObject);
		}
		fruits.Clear();
		return fruitNames.ToArray();
	}

	public string GetLabel()
	{
		if (activeLabel == null) return null;

		return activeLabel.transform.Find("text").GetComponent<TextMesh>().text;
	}
}
