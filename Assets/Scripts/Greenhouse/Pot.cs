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
			plant = Instantiate(plantPrefab);
			//TODO: position
			Plant plantScript = plant.GetComponent<Plant>();
			plantScript.SetGenome(fruit.GetGenome());
		}
	}
}
