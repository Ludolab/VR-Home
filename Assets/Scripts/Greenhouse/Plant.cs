using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{

	public GameObject[] growthStages;
	public int nonFruitingStages; //1 for seed only, 2 for seed and one non-fruiting young stage, etc.

	private GameObject model;
	private int stage;
	private int dayBorn;

	private void Start()
	{
		TimeManager.instance.AddPlant(this);
		dayBorn = TimeManager.instance.GetDay();
		stage = 0;
		UpdateModel();
	}
	
	private void UpdateModel()
	{
		if (model != null)
		{
			Destroy(model);
		}
		model = Instantiate(growthStages[stage], transform.position, Quaternion.identity);

		//TODO: spawn fruit- half ripe and half unripe (don't spawn unripe if last ripe one was not picked)
	}

	public void StartDay(int day)
	{
		stage = day - dayBorn;
		if (stage >= growthStages.Length)
		{
			stage = nonFruitingStages;
		}
		UpdateModel();
	}
}
