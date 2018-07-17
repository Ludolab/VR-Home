using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour {

    public GameObject beetlePrefab;
    public GameObject weedPrefab;
    public GameObject myDirt;
    public int maxWeeds;

    private Vector3 center;
    private float radiusX;
    private float radiusZ;
    private Plant plant = null;
    private Dictionary<GameObject, int> beetles = new Dictionary<GameObject, int>(); //Keep track of beetles and which instance (of position) it is.
    private Dictionary<GameObject, int> fruits = new Dictionary<GameObject, int>(); //Keep track of fruit and which instance (of position) it is.
    private List<GameObject> weeds = new List<GameObject>(); //Keep track of weeds.

	private void Start()
	{
        // Store some info about the dirt plot area for spawning things in.
        Collider col = GetComponent<Collider>();
        if(col != null) {
            center = col.bounds.center;
            radiusX = col.bounds.extents.x;
            radiusZ = col.bounds.extents.z;
        }
	}

	//TODO: change this to be after dirt planting action has been done.
    private void OnTriggerEnter(Collider other)
	{
        Debug.Log("Plot collided with.");
        Debug.Log(other.gameObject.name);
        Plant p = other.gameObject.GetComponent<Plant>();
        if(p == null) Debug.Log("Not a plant...");
        if(plant == null && p != null && p.getStage() == 0) {
            Debug.Log("Found a new plant.");
            //TODO: make it so plot cannot be dug anymore when it has a plant in it.
            setPlant(p);
        }
	}

    // TODO: picking a plant.

    // This needs to be revised later.
	public void setPlant(Plant p) {
        plant = p;
        // Snap plant to the center of the plot.
        // TODO: disable gravity on object (seed starter?) with plant component.
        plant.transform.position = center;
        plant.transform.eulerAngles = new Vector3(0, 0, 0);
        plant.transform.localScale = new Vector3(1, 1, 1);
        plant.PlantPlant();
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
        foreach(GameObject weed in weeds) {
            Destroy(weed);
        }
        beetles = new Dictionary<GameObject, int>();
        fruits = new Dictionary<GameObject, int>();
        weeds = new List<GameObject>();

        // TODO: condition on stage based on watering.
        // TODO: update plot lists/dictionaries when beetles/weeds/fruit have been squished/pulled up/picked.
        if (plant != null && beetles.Count == 0 && weeds.Count == 0)
        {
            Debug.Log("Advancing stage on plant.");
            plant.advanceStage();
        }

        // Spawn in weeds.
        if(weeds.Count < maxWeeds) {
            Debug.Log("Spawning in new weeds.");
            int numberWeeds = (int)(Random.Range(0f, maxWeeds - weeds.Count));
            for (int i = 0; i < numberWeeds; i++)
            {
                //TODO: make it so weeds don't spawn too close to each other.
                GameObject weed = Instantiate(weedPrefab);
                float xPos = Random.Range(-1 * radiusX, radiusX) + center.x;
                float zPos = Random.Range(-1 * radiusZ, radiusZ) + center.z;
                float scale = Random.Range(0.9f, 1.5f);
                weed.transform.position = new Vector3(xPos, center.y, zPos);
                weed.transform.eulerAngles = new Vector3(Random.Range(-5f, 5), Random.Range(0f, 360f), Random.Range(-5f, 5));
                weed.transform.localScale = new Vector3(scale, scale, scale);

                // Keep track of spawned weeds.
                weeds.Add(weed);
            }
        }

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

    //Keep states of plot up-to-date.
    public void removeFromWeeds(GameObject weed)
    {
        weeds.Remove(weed);
    }

    public void removeFromBeetles(GameObject beetle)
    {
        beetles.Remove(beetle);
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
