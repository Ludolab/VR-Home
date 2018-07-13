using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour {

    public GameObject beetlePrefab;

    private bool hasPlant;
    private Plant plant;

    //TODO: change this later to be after dirt planting action has been done.
	void OnCollisionEnter(Collision col)
	{
        Plant p = col.gameObject.GetComponent<Plant>();
        if(!hasPlant && p != null) {
            hasPlant = true;
            plant = p;
            p.Planted();
        }
	}

    public void StartDay(int day)
    {
        if(hasPlant) {
            //TODO: condition on stage based on watering, beetle squashing, weeding.
            plant.setStage(day - plant.getDayBorn());
            if (plant.getStage() >= plant.growthStages.Length)
            {
                plant.setStage(plant.nonFruitingStages);
            }
            plant.UpdateModel();

            //TODO: adding weeds randomly.

            // Check for the young stage.
            if(plant.getStage() == 1)
            {
                //TODO: don't spawn more beetles if the previous day's haven't been killed.
                //TODO: come up with a better system for randomizing number of beetles?
                int numberBeetles = (int)(Random.Range(-1f, plant.beetleTransYoung.Length - 1));
                for (int i = 0; i <= numberBeetles; i++)
                {
                    GameObject beetle = Instantiate(beetlePrefab);
                    beetle.transform.position = plant.transform.position + plant.beetleTransYoung[i].localPosition;
                    beetle.transform.eulerAngles = plant.transform.eulerAngles + plant.beetleTransYoung[i].localEulerAngles;
                    beetle.transform.localScale = plant.beetleTransYoung[i].localScale;
                }
            }

            // Check for the grown stage.
            // Note should not be proceeded to if any beetles remained on the young stage, so shouldn't have conflicts there in final.
            if(plant.getStage() == 2)
            {
                int numberBeetles = (int)(Random.Range(-1f, plant.beetleTransGrown.Length - 1));
                for (int i = 0; i <= numberBeetles; i++)
                {
                    GameObject beetle = Instantiate(beetlePrefab);
                    beetle.transform.position = plant.transform.position + plant.beetleTransGrown[i].localPosition;
                    beetle.transform.eulerAngles = plant.transform.eulerAngles + plant.beetleTransGrown[i].localEulerAngles;
                    beetle.transform.localScale = plant.beetleTransGrown[i].localScale;
                }

                // Check if we should spawn in fruit.
                if(plant.multiHarvest) {
                    int numberFruit = (int)(Random.Range(-1f, plant.fruitTrans.Length - 1));
                    for (int i = 0; i <= numberFruit; i++)
                    {
                        GameObject fruit = Instantiate(plant.fruit);
                        fruit.transform.position = plant.transform.position + plant.fruitTrans[i].localPosition;
                        fruit.transform.eulerAngles = plant.transform.eulerAngles + plant.fruitTrans[i].localEulerAngles;
                        fruit.transform.localScale = plant.fruitTrans[i].localScale;
                    }
                }
            }
        }
    }
}
