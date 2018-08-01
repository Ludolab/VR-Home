using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour {

    public InteractionManager manager;
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

        TimeManager.instance.RemoveStarter(mySeedCollider.myStarter);
        Destroy(mySeedCollider.myStarter.gameObject);
    }

    public void RemovePlant() {
        Destroy(plant);
        plant = null;
        myDirt.makeFlat(true);
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
        if (plant != null)
        {
            // Make sure we can't plant again in this dirt so long as the plant remains.
            myDirt.makeFlat(false);

            if (beetles.Count == 0 && weeds.Count == 0 && myDirt.getWetness() > 0.7f) plant.AdvanceStage();

            // Check if we should spawn in fruit.
            if (plant.GetStage() == plant.nonFruitingStages)
            {
                if (plant.multiHarvest && beetles.Count == 0 && beetles.Count == 0 && weeds.Count == 0 && myDirt.getWetness() > 0.7f)
                {
                    SpawnFruit(plant.fruitTrans);
                }
                else
                {
                    SetHarvest();
                }
            }

            // Check for stage and spawn in appropriate beetles.
            // Note since the plant won't grow if any beetles remained, we shouldn't have conflicts.
            if (plant.GetStage() == 1 && plant.beetleTrans1 != null)
            {
                SpawnBeetles(plant.beetleTrans1);
            } else if (plant.GetStage() == 2 && plant.beetleTrans2 != null)
            {
                SpawnBeetles(plant.beetleTrans2);
            }


        } else {
            // If there's no plant left in the plot, make it replantable the next day.
            myDirt.makeFlat(true);
        }

        // Add new weeds.
        if (weeds.Count < maxWeeds)
        {
            SpawnWeeds();
        }

        // Reset watering.
        myDirt.setWetness(0f);
    }

    public void SetHarvest() {
        HarvestFruit harvestable = plant.GetModel().GetComponent<HarvestFruit>();
        if (harvestable != null)
        {
            harvestable.SetPlot(this);
        }
    }



    private void SpawnBeetles(Transform[] transforms) {
        //Range from no beetles (-1) to maximum placements.
        int numberBeetles = (int)(Random.Range(-1f, transforms.Length - 1));
        for (int i = 0; i <= numberBeetles; i++)
        {
            //Make sure we don't spawn in duplicates.
            if (!beetles.ContainsValue(i))
            {
                GameObject beetle = Instantiate(beetlePrefab);
                beetle.transform.position = plant.transform.position + transforms[i].localPosition;
                beetle.transform.eulerAngles = plant.transform.eulerAngles + transforms[i].localEulerAngles;
                beetle.transform.localScale = transforms[i].localScale;

                beetle.GetComponent<Beetle>().setPlot(this);
                beetles.Add(beetle, i);
            }
        }
    }

    private void SpawnFruit(Transform[] transforms) {
        //Range from one fruit to maximum placements.
        int numberFruit = (int)(Random.Range(1f, transforms.Length - 1));
        for (int i = 0; i <= numberFruit; i++)
        {
            if (!fruits.ContainsValue(i))
            {
                GameObject fruit = Instantiate(plant.fruit, plant.transform.position + transforms[i].localPosition, Quaternion.identity);
                fruit.transform.eulerAngles = plant.transform.eulerAngles + transforms[i].localEulerAngles;
                fruit.transform.localScale = transforms[i].localScale;

                fruit.GetComponent<Fruit>().setPlot(this);
                fruit.GetComponent<InteractionBehaviour>().manager = manager;
                fruits.Add(fruit, i);
            }
        }
    }

    private void SpawnWeeds() {
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



    //Keep states of plot up-to-date.
    public void RemoveFromWeeds(GameObject weed)
    {
        weeds.Remove(weed);
        if (weeds.Count == 0) myDirt.noWeeds = true;
    }

    public void RemoveFromBeetles(GameObject beetle)
    {
        beetles.Remove(beetle);
    }

    public void SquishBeetles() {
        GameObject[] toSquish = new GameObject[beetles.Keys.Count];
        beetles.Keys.CopyTo(toSquish, 0);
        foreach (GameObject beetle in toSquish) {
            beetle.GetComponent<Beetle>().Squish();
        }
    }

    public void RemoveFromFruits(GameObject fruit)
    {
        fruits.Remove(fruit);
    }



    //Following are all data modification functions to be used by saving/loading (primarily).
    public void SetPlant(Plant p, int stage)
    {
        plant = p;
        plant.transform.position = gameObject.transform.position;
        plant.transform.eulerAngles = new Vector3(0, 0, 0);
        plant.transform.localScale = new Vector3(1, 1, 1);
        plant.SetStage(stage);
    }

    public void AddToWeeds(GameObject weed) {
        weeds.Add(weed);
    }

    public void AddToBeetles(GameObject beetle, int instance) {
        beetles.Add(beetle, instance);
    }

    public void AddToFruit(GameObject fruit, int instance) {
        fruits.Add(fruit, instance);
    }

    public Plant GetPlant()
    {
        return plant;
    }

    public List<GameObject> GetWeeds() {
        return weeds;
    }

    public int[] GetBeetleIDs() {
        ICollection currentBeetles = beetles.Values;
        int[] ids = new int[currentBeetles.Count];
        currentBeetles.CopyTo(ids, 0);
        return ids;

    }

    public int[] GetFruitIDs() {
        ICollection currentFruit = fruits.Values;
        int[] ids = new int[currentFruit.Count];
        currentFruit.CopyTo(ids, 0);
        return ids;
    }
}
