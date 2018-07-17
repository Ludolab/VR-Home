using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour {

    public GameObject beetlePrefab;
    public GameObject weedPrefab;
    public GameObject myDirt;
    public int maxWeeds;

    private Vector3 center;
    private Plant plant;
    private Dictionary<GameObject, int> beetles = new Dictionary<GameObject, int>(); //Keep track of beetles and which instance (of position) it is.
    private Dictionary<GameObject, int> fruits = new Dictionary<GameObject, int>(); //Keep track of fruit and which instance (of position) it is.


	private void Start()
	{
        Collider col = GetComponent<Collider>();
        if(col != null) {
            center = col.bounds.center;
        }
	}



	//TODO: change this to be after dirt planting action has been done.
	void OnTriggerEnter(Collider other)
	{
        Debug.Log("Plot collided with.");
        Plant p = other.gameObject.GetComponent<Plant>();
        if(plant == null && p != null) {
            Debug.Log("Found a plant.");
            setPlant(p);
        }
	}

    public void setPlant(Plant p) {
        //TODO: make it so plot cannot be dug anymore when it has a plant in it.
        plant = p;
        // Snap plant to the center of the plot.
        plant.transform.position = center;
        plant.transform.eulerAngles = new Vector3(0, 0, 0);
        plant.transform.localScale = new Vector3(1, 1, 1);
    }

    public void StartDay()
    {
        // For temporary debuging purposes, clear everything automatically at the start of the day.
        foreach(GameObject beetle in beetles.Keys) {
            Destroy(beetle);
        }
        foreach(GameObject fruit in fruits.Keys) {
            Destroy(fruit);
        }
        beetles = new Dictionary<GameObject, int>();
        fruits = new Dictionary<GameObject, int>();

        // TODO: condition on stage based on watering, weeds.
        // TODO: update when beetles/weeds/fruit have been squished/pulled up/picked.
        // Q: update only at end of day (self-contained in Plot) or in real-time (beetle/weed/fruit removes itself from Plot
        // as it is squished/pulled up/picked)? Is there relevance in tracking the state of a plot beyond when a day is ended or loaded?
        if (plant != null && beetles.Count == 0)
        {
            Debug.Log("Advancing stage on plant.");
            plant.advanceStage();
        }

        //TODO: add weed spawning.

        // Check for the young stage.
        if(plant != null && plant.getStage() == 1)
        {
            Debug.Log("Young stage. Adding beetles.");

            //Range from no beetles (-1) to maximum placements.
            int numberBeetles = (int)(Random.Range(-1f, plant.beetleTransYoung.Length - 1));
            for (int i = 0; i <= numberBeetles; i++)
            {
                //Make sure we don't spawn in duplicates.
                if(!beetles.ContainsValue(i)) {
                    GameObject beetle = Instantiate(beetlePrefab);
                    beetle.transform.position = plant.transform.position + plant.beetleTransYoung[i].localPosition;
                    beetle.transform.eulerAngles = plant.transform.eulerAngles + plant.beetleTransYoung[i].localEulerAngles;
                    beetle.transform.localScale = plant.beetleTransYoung[i].localScale;
                    //Keep track of spawned beetles.
                    beetles.Add(beetle, i);
                }
            }
        }

        // Check for the grown stage.
        // Note should not be proceeded to if any beetles remained on the young stage, so shouldn't have conflicts with beetle spawning.
        if(plant != null && plant.getStage() >= 2)
        {
            Debug.Log("Grown stage. Adding beetles.");

            int numberBeetles = (int)(Random.Range(-1f, plant.beetleTransGrown.Length - 1));
            for (int i = 0; i <= numberBeetles; i++)
            {
                if (!beetles.ContainsValue(i))
                {
                    GameObject beetle = Instantiate(beetlePrefab);
                    beetle.transform.position = plant.transform.position + plant.beetleTransGrown[i].localPosition;
                    beetle.transform.eulerAngles = plant.transform.eulerAngles + plant.beetleTransGrown[i].localEulerAngles;
                    beetle.transform.localScale = plant.beetleTransGrown[i].localScale;
                    beetles.Add(beetle, i);
                }
            }

            // Check if we should spawn in fruit.
            if(plant.multiHarvest) {
                Debug.Log("Grown stage on multiharvest plant. Adding fruit to harvest.");

                //Range from one fruit to maximum placements.
                int numberFruit = (int)(Random.Range(0f, plant.fruitTrans.Length - 1));
                for (int i = 0; i <= numberFruit; i++)
                {
                    if(!fruits.ContainsValue(i)) {
                        GameObject fruit = Instantiate(plant.fruit);
                        fruit.transform.position = plant.transform.position + plant.fruitTrans[i].localPosition;
                        fruit.transform.eulerAngles = plant.transform.eulerAngles + plant.fruitTrans[i].localEulerAngles;
                        fruit.transform.localScale = plant.fruitTrans[i].localScale;
                        fruits.Add(fruit, i);
                    }
                }
            }
        }
    }

    public void setPlant(Plant p, int stage) {
        plant = p;
        plant.setStage(stage);
    }

    public Plant getPlant() {
        return plant;
    }

    //For when loading the next day from new play session, keep track of beetles/weeds/fruit that weren't squished/pulled up/picked previously.
    public void addToBeetles(GameObject beetle, int instance) {
        beetles.Add(beetle, instance);
    }

    public void addToFruit(GameObject fruit, int instance) {
        fruits.Add(fruit, instance);
    }

    public int[] getBeetleIDs() {
        ICollection currentBeetles = beetles.Values;
        int[] ids = new int[currentBeetles.Count];
        currentBeetles.CopyTo(ids, 0);
        return ids;

    }

    public int[] getFruitIDs() {
        ICollection currentFruit = fruits.Values;
        int[] ids = new int[currentFruit.Count];
        currentFruit.CopyTo(ids, 0);
        return ids;
    }

    public void removeFromBeetles(GameObject beetle) {
        beetles.Remove(beetle);
    }

    public void removeFromFruits(GameObject fruit) {
        fruits.Remove(fruit);
    }
}
