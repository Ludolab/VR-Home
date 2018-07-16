﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{

	public GameObject[] growthStages;
    public Transform[] growthStagesTrans; //List of transforms corresponding to the growth stages.
    public bool multiHarvest; //For plants that bear fruit that can be picked without taking the entire plant.
    public GameObject fruit; //Model of fruit (if multiHarvest plant).
    public Transform[] fruitTrans; //Where fruit can spawn (if multiHarvest plant), locally.
    public Transform[] beetleTransYoung; //Where beetles can spawn, on young stage, locally.
    public Transform[] beetleTransGrown; //Where beetles can spawn, on young stage, locally.
	public int nonFruitingStages; //1 for seed only, 2 for seed and one non-fruiting young stage, etc.

	private GameObject model;
	private int stage;
	private int dayBorn;

	public void PlantPlant()
	{
        Debug.Log("Plant planted. Now growing.");
		//TimeManager.instance.AddPlant(this);
		dayBorn = TimeManager.instance.GetDay();
		stage = 0;
		UpdateModel();
	}
	
	public void UpdateModel()
	{
        Debug.Log("Updating plant model.");
		if (model != null)
		{
			Destroy(model);
        }

        model = Instantiate(growthStages[stage]);
        model.transform.position = transform.position + growthStagesTrans[stage].localPosition;
        model.transform.eulerAngles = transform.eulerAngles + growthStagesTrans[stage].localEulerAngles;
        model.transform.localScale = growthStagesTrans[stage].localScale;

		//TODO: spawn fruit- half ripe and half unripe (don't spawn unripe if last ripe one was not picked)
	}

    public void advanceStage() {
        stage++;
        if (stage >= growthStages.Length)
        {
            stage = nonFruitingStages;
        }
        UpdateModel();
    }

    public void setStage(int s) {
        stage = s;
        UpdateModel();
    }

    public int getStage() {
        return stage;
    }

    public void setDayBorn(int db) {
        dayBorn = db;
    }

    public int getDayBorn() {
        return dayBorn;
    }
}
