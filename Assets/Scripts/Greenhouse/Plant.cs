using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public string plant;
	public GameObject[] growthStages;
    public Transform[] growthStagesTrans; //List of transforms corresponding to the growth stages.
    public bool multiHarvest; //For plants that bear fruit that can be picked without taking the entire plant.
    public GameObject fruit; //Model of fruit (if multiHarvest plant).
    public Transform[] fruitTrans; //Where fruit can spawn (if multiHarvest plant), locally.
    public Transform[] beetleTrans1; //Where beetles can spawn, on stage 1, locally.
    public Transform[] beetleTrans2; //Where beetles can spawn, on stage 2, locally.
	public int nonFruitingStages; //1 for seed only, 2 for seed and one non-fruiting young stage, etc.

	private GameObject model;
	private int stage = 0;
	private int dayBorn;

	public void PlantPlant()
	{
		//TimeManager.instance.AddPlant(this);
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

        model = Instantiate(growthStages[stage], transform.position + growthStagesTrans[stage].localPosition, Quaternion.identity);
        model.transform.eulerAngles = transform.eulerAngles + growthStagesTrans[stage].localEulerAngles;
        model.transform.localScale = growthStagesTrans[stage].localScale;
	}

    public void AdvanceStage() {
        stage++;
        if (stage >= growthStages.Length)
        {
            stage = nonFruitingStages;
        }
        UpdateModel();
    }

    public void SetStage(int s) {
        stage = s;
        UpdateModel();
    }

    public int GetStage() {
        return stage;
    }

    public void SetDayBorn(int db) {
        dayBorn = db;
    }

    public int GetDayBorn() {
        return dayBorn;
    }

    public GameObject GetModel() {
        return model;
    }
}
