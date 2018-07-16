using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour {

    public GameObject beetlePrefab;
    public GameObject weedPrefab;

    private Vector3 center;
    private float boundX;
    private float boundZ;
    private bool hasPlant;
    private Plant plant;
    private Dictionary<int, GameObject> beetles = new Dictionary<int, GameObject>(); //Keep track of beetles and which instance (of position) it is.
    private List<GameObject> weeds = new List<GameObject>();
    private Dictionary<int, GameObject> fruits = new Dictionary<int, GameObject>(); //Keep track of fruit and which instance (of position) it is.

	private void Start()
	{
        Collider col = GetComponent<Collider>();
        if(col != null) {
            center = col.bounds.center;
            boundX = col.bounds.extents.x;
            boundZ = col.bounds.extents.z;
        }
	}

	//TODO: change this to be after dirt planting action has been done.
	void OnTriggerEnter(Collision other)
	{
        Debug.Log("Plot collided with.");
        Plant p = other.gameObject.GetComponent<Plant>();
        if(!hasPlant && p != null) {
            Debug.Log("Found a plant.");
            hasPlant = true;
            plant = p;
            p.PlantPlant();
        }
	}

    public void StartNextDay()
    {
        //For temporary debuging purposes, clear everything automatically at the start of the day.
        beetles = new Dictionary<int, GameObject>();
        weeds = new List<GameObject>();
        fruits = new Dictionary<int, GameObject>();

        //TODO: condition on stage based on watering.
        //TODO: update when beetles/weeds/fruit have been squished/pulled up/picked.
        if(hasPlant && plant != null && beetles.Count == 0 && weeds.Count == 0) {
            Debug.Log("Advancing stage on plant.");
            plant.advanceStage();

            //Add weeds within the bounds of the plot.
            //Range from no weeds (-1) to 3 weeds per plot.
            int numberWeeds = (int)(Random.Range(-1f, 3f));
            for (int i = 0; i <= numberWeeds; i++)
            {
                //TODO: make sure that weeds don't spawn at the same/very close transform positions.
                Vector3 spawnPoint = new Vector3();
                spawnPoint.x = center.x + Random.Range(-1 * boundX, boundX);
                spawnPoint.y = center.y;
                spawnPoint.z = center.z + Random.Range(-1 * boundZ, boundZ);
                GameObject weed = Instantiate(weedPrefab);
                weed.transform.position = spawnPoint;
                //Keep track of spawned weeds.
                weeds.Add(weed);
            }

            // Check for the young stage.
            if(plant.getStage() == 1)
            {
                Debug.Log("Young stage. Adding beetles.");
                //TODO: don't spawn more beetles if the previous day's haven't been killed.
                //TODO: come up with a better system for randomizing number of beetles?
                //TODO: implement a tracker for whether each beetle has been squished or not.

                //Range from no beetles (-1) to maximum placements.
                int numberBeetles = (int)(Random.Range(-1f, plant.beetleTransYoung.Length - 1));
                for (int i = 0; i <= numberBeetles; i++)
                {
                    if(!beetles.ContainsKey(i)) {
                        GameObject beetle = Instantiate(beetlePrefab);
                        beetle.transform.position = plant.transform.position + plant.beetleTransYoung[i].localPosition;
                        beetle.transform.eulerAngles = plant.transform.eulerAngles + plant.beetleTransYoung[i].localEulerAngles;
                        beetle.transform.localScale = plant.beetleTransYoung[i].localScale;
                        //Keep track of spawned beetles.
                        beetles.Add(i, beetle);
                    }
                }
            }

            // Check for the grown stage.
            // Note should not be proceeded to if any beetles remained on the young stage, so shouldn't have conflicts there.
            if(plant.getStage() == 2)
            {
                Debug.Log("Grown stage. Adding beetles.");

                int numberBeetles = (int)(Random.Range(-1f, plant.beetleTransGrown.Length - 1));
                for (int i = 0; i <= numberBeetles; i++)
                {
                    if (!beetles.ContainsKey(i))
                    {
                        GameObject beetle = Instantiate(beetlePrefab);
                        beetle.transform.position = plant.transform.position + plant.beetleTransGrown[i].localPosition;
                        beetle.transform.eulerAngles = plant.transform.eulerAngles + plant.beetleTransGrown[i].localEulerAngles;
                        beetle.transform.localScale = plant.beetleTransGrown[i].localScale;
                        beetles.Add(i, beetle);
                    }
                }

                // Check if we should spawn in fruit.
                if(plant.multiHarvest) {
                    Debug.Log("Grown stage on multiharvest plant. Adding fruit to harvest.");

                    //Range from one fruit to maximum placements.
                    int numberFruit = (int)(Random.Range(0f, plant.fruitTrans.Length - 1));
                    for (int i = 0; i <= numberFruit; i++)
                    {
                        if(!fruits.ContainsKey(i)) {
                            GameObject fruit = Instantiate(plant.fruit);
                            fruit.transform.position = plant.transform.position + plant.fruitTrans[i].localPosition;
                            fruit.transform.eulerAngles = plant.transform.eulerAngles + plant.fruitTrans[i].localEulerAngles;
                            fruit.transform.localScale = plant.fruitTrans[i].localScale;
                            fruits.Add(i, fruit);
                        }
                    }
                }
            }
        }
    }

    //For when loading the next day from new play session, keep track of beetles/weeds/fruit that weren't squished/pulled up/picked previously.
    public void addToWeeds(GameObject weed) {
        weeds.Add(weed);
    }

    public void addToBeetles(int instance, GameObject beetle) {
        beetles.Add(instance, beetle);
    }

    public void addToFruit(int instance, GameObject fruit) {
        fruits.Add(instance, fruit);
    }
}
