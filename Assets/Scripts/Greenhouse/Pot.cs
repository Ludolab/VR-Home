using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
	public GameObject plantPrefab;

	private GameObject plant;

	public void PlantFruit(Fruit fruit)
	{
		if (plant == null)
		{
			plant = Instantiate(plantPrefab, transform.position, Quaternion.identity);
			GrowingPlant plantScript = plant.GetComponent<GrowingPlant>();
			//plantScript.SetGenome(fruit.GetGenome());
		}
	}
}
