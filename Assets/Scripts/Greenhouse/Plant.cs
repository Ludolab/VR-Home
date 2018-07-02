using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{

	public GameObject fruitPrefab;

	private Genome genome;

	public void SetGenome(Genome g)
	{
		genome = g;
	}

	private void Update()
	{
		//TODO: plant growing stuff
	}

	private GameObject CreateFruit()
	{
		//create fruit with randomized genome
		GameObject fruitObj = Instantiate(fruitPrefab);
		Fruit fruit = fruitObj.GetComponent<Fruit>();

		Genome g = MutateGenome();
		fruit.SetGenome(g);

		return fruitObj;
	}

	private Genome MutateGenome()
	{
		//TODO: randomize some property a little bit
		return genome;
	}

}
