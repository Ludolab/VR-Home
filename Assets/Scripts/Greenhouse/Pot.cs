using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
	public GameObject fruitPrefab;

	private Genome plantedGenome;
	private bool isFilled = false;

	public void PlantFruit(Fruit fruit)
	{
		if (!isFilled)
		{
			plantedGenome = fruit.GetGenome();
			isFilled = true;
		}
	}

	private void Update()
	{
		if (isFilled)
		{
			GameObject fruit = Instantiate(fruitPrefab, transform.position, Quaternion.identity);
			Fruit fruitInfo = fruit.GetComponent<Fruit>();
			fruitInfo.SetGenome(plantedGenome);
		}
	}
}
