using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour {

    public GameObject beetlePrefab;
    public GameObject weedPrefab;
    public Dirt myDirt;
    public SeedCollider mySeedCollider;
    public int maxWeeds;

    //Currently hard-coded since there's issues with pulling collider bound radius for some reason.
    public float radiusX;
    public float radiusZ;

    public Plant plant = null;
    private Dictionary<GameObject, int> beetles = new Dictionary<GameObject, int>(); //Keep track of beetles and which instance (of position) it is.
    private Dictionary<GameObject, int> fruits = new Dictionary<GameObject, int>(); //Keep track of fruit and which instance (of position) it is.
    private List<GameObject> weeds = new List<GameObject>(); //Keep track of weeds.

    public void AbsorbPlant(){
        GameObject planted = (GameObject)Instantiate(Resources.Load("Prefabs/Plants/" + mySeedCollider.myStarter.plantName));
        plant = planted.GetComponent<Plant>();
        planted.transform.position = gameObject.transform.position;
        planted.transform.eulerAngles = new Vector3(0, 0, 0);
        planted.transform.localScale = new Vector3(1, 1, 1);
        plant.PlantPlant();
        Destroy(mySeedCollider.myStarter.gameObject);
    }

	/* EVERYTHING BELOW WAS REPLACED BY "absorbPlant". I am commenting it in case we need any of it later.
    private void OnTriggerEnter(Collider other)
	{
        Plant p = other.gameObject.GetComponent<Plant>();
        if(plant == null && p != null && p.getStage() == 0) {
            absorbPlant(p);
        }
	}

    // This needs to be revised later.
	public void absorbPlant(Plant p) {
        plant = p;
        // Snap plant to the center of the plot.
        plant.transform.position = center;
        plant.transform.eulerAngles = new Vector3(0, 0, 0);
        plant.transform.localScale = new Vector3(1, 1, 1);
        plant.PlantPlant();
    } */

	public void StartDay()
    {
        /*// For temporary debuging purposes, clear everything automatically at the start of the day.
        // Mostly, since we have yet to add tracking when beetles and weeds are removed.
        foreach (GameObject beetle in beetles.Keys)
        {
            Destroy(beetle);
        }
        foreach (GameObject fruit in fruits.Keys)
        {
            Destroy(fruit);
        }
        foreach (GameObject weed in weeds)
        {
            Destroy(weed);
        }
        beetles = new Dictionary<GameObject, int>();
        fruits = new Dictionary<GameObject, int>();
        weeds = new List<GameObject>();*/

        if (plant != null)
        {
            myDirt.makeFlat(false);

            if (/*beetles.Count == 0*/ && weeds.Count == 0 && myDirt.getWetness() > 0.7f) plant.advanceStage();

            // Check for the young stage.
            if (plant.getStage() == 1)
            {
                //Range from no beetles (-1) to maximum placements.
                int numberBeetles = (int)(Random.Range(-1f, plant.beetleTransYoung.Length - 1));
                for (int i = 0; i <= numberBeetles; i++)
                {
                    //Make sure we don't spawn in duplicates.
                    if (!beetles.ContainsValue(i))
                    {
                        GameObject beetle = Instantiate(beetlePrefab);
                        beetle.transform.position = plant.transform.position + plant.beetleTransYoung[i].localPosition;
                        beetle.transform.eulerAngles = plant.transform.eulerAngles + plant.beetleTransYoung[i].localEulerAngles;
                        beetle.transform.localScale = plant.beetleTransYoung[i].localScale;

                        beetle.GetComponent<Beetle>().setPlot(this);
                        beetles.Add(beetle, i);
                    }
                }
            }

            // Check for the grown stage.
            // Note should not be proceeded to if any beetles remained on the young stage, so shouldn't have conflicts with beetle spawning.
            if (plant.getStage() >= 2)
            {
                int numberBeetles = (int)(Random.Range(-1f, plant.beetleTransGrown.Length - 1));
                for (int i = 0; i <= numberBeetles; i++)
                {
                    if (!beetles.ContainsValue(i))
                    {
                        GameObject beetle = Instantiate(beetlePrefab);
                        beetle.transform.position = plant.transform.position + plant.beetleTransGrown[i].localPosition;
                        beetle.transform.eulerAngles = plant.transform.eulerAngles + plant.beetleTransGrown[i].localEulerAngles;
                        beetle.transform.localScale = plant.beetleTransGrown[i].localScale;

                        beetle.GetComponent<Beetle>().setPlot(this);
                        beetles.Add(beetle, i);
                    }
                    if (numberBeetles > -1 && !plant.multiHarvest) plant.getModel().GetComponent<HarvestFruit>().setNoBeetles(false);
                }
            }

            // Check if we should spawn in fruit.
            if (plant.getStage() > plant.nonFruitingStages)
            {
                if (plant.multiHarvest)
                {
                    //Range from one fruit to maximum placements.
                    int numberFruit = (int)(Random.Range(1f, plant.fruitTrans.Length - 1));
                    for (int i = 0; i <= numberFruit; i++)
                    {
                        if (!fruits.ContainsValue(i))
                        {
                            GameObject fruit = Instantiate(plant.fruit);
                            fruit.transform.position = plant.transform.position + plant.fruitTrans[i].localPosition;
                            fruit.transform.eulerAngles = plant.transform.eulerAngles + plant.fruitTrans[i].localEulerAngles;
                            fruit.transform.localScale = plant.fruitTrans[i].localScale;

                            fruit.GetComponent<Fruit>().setPlot(this);
                            fruits.Add(fruit, i);
                        }
                    }
                } else
                {
                    plant.GetComponent<HarvestFruit>().setPlot(this);
                }
            }

        } else {
            myDirt.makeFlat(true);
        }

        // Add new weeds.
        if (weeds.Count < maxWeeds)
        {
            int numberWeeds = (int)(Random.Range(1f, maxWeeds - weeds.Count));
            for (int i = 0; i < numberWeeds; i++)
            {
                GameObject weed = Instantiate(weedPrefab);
                float xPos = Random.Range(-1 * radiusX, radiusX) + gameObject.transform.position.x;
                float zPos = Random.Range(-1 * radiusZ, radiusZ) + gameObject.transform.position.z;
                float scale = Random.Range(0.9f, 1.5f);
                weed.transform.position = new Vector3(xPos, gameObject.transform.position.y, zPos);
                weed.transform.eulerAngles = new Vector3(Random.Range(-5f, 5), Random.Range(0f, 360f), Random.Range(-5f, 5));
                weed.transform.localScale = new Vector3(scale, scale, scale);

                // Keep track of spawned weeds.
                weed.GetComponent<PullWeed>().setPlot(this);
                weeds.Add(weed);
            }
            // Don't let people dig dirt until all weeds are gone.
            myDirt.noWeeds = false;
        }

        // Reset watering.
        myDirt.resetWetness();
    }

    //Keep states of plot up-to-date.
    public void removeFromWeeds(GameObject weed)
    {
        weeds.Remove(weed);
        if (weeds.Count == 0) myDirt.noWeeds = true;
    }

    public void removeFromBeetles(GameObject beetle)
    {
        beetles.Remove(beetle);
        if (plant.getStage() > plant.nonFruitingStages && !plant.multiHarvest && beetles.Count == 0)
            plant.GetComponent<HarvestFruit>().setNoBeetles(true);
    }

    public void removeFromFruits(GameObject fruit)
    {
        fruits.Remove(fruit);
    }

    //Following are all data modification functions to be used by saving/loading (primarily).
    public void setPlant(Plant p, int stage) {
        plant = p;
        plant.setStage(stage);
    }

    public Plant getPlant() {
        return plant;
    }

    public void addToWeeds(GameObject weed) {
        weeds.Add(weed);
    }

    public void addToBeetles(GameObject beetle, int instance) {
        beetles.Add(beetle, instance);
    }

    public void addToFruit(GameObject fruit, int instance) {
        fruits.Add(fruit, instance);
    }

    public List<GameObject> getWeeds() {
        return weeds;
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
}
