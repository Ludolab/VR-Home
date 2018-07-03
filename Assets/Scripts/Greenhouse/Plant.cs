using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{

	public GameObject fruitPrefab;

	public float stemAge = 0.3f;
	public float branchAge = 0.7f;
	public float fruitAge = 1.0f;

	public GameObject stem;
	public GameObject branch;
	public Transform fruitPos;

	private Genome genome;
	private float age; //0 to 1
	private Fruit fruit;
	private bool complete;

	private void Start()
	{
		age = 0;
		complete = false;
	}

	public void SetGenome(Genome g)
	{
		genome = g;
	}

	private void Update()
	{
		//grow the plant

		if (!complete)
		{
			age += Time.deltaTime / genome.lifetime;
		}
		
		if (age < stemAge)
		{
			float stemScale = GetScaledTime(age, 0, stemAge);
			GrowStem(stemScale);
		}
		else if (age < branchAge)
		{
			if (!branch.activeSelf)
			{
				GrowStem(1);
				branch.SetActive(true);
			}
			
			float branchScale = GetScaledTime(age, stemAge, branchAge);
			GrowBranch(branchScale);
		}
		else if (age < fruitAge)
		{
			//grow fruit
			if (fruit == null)
			{
				GrowBranch(1);
				fruit = CreateFruit();
			}
			float fruitScale = GetScaledTime(age, branchAge, fruitAge);
			GrowFruit(fruitScale);
		}
		else if (!complete)
		{
			GrowFruit(1);
			complete = true;
		}
	}

	private static float GetScaledTime(float t, float prevT, float nextT)
	{
		return (t - prevT) / (nextT - prevT);
	}

	private void GrowStem(float stemScale)
	{
		float stemY = stemScale * 0.3f;
		Vector3 stemPos = stem.transform.localPosition;
		stemPos.y = stemY;
		stem.transform.localPosition = stemPos;
		stem.transform.localScale = new Vector3(stemScale * 0.1f, stemScale * 0.3f, stemScale * 0.1f);
	}

	private void GrowBranch(float branchScale)
	{
		float branchX = branchScale * 0.3f;
		Vector3 branchPos = branch.transform.localPosition;
		branchPos.x = branchX;
		branch.transform.localPosition = branchPos;
		branch.transform.localScale = new Vector3(branchScale * 0.1f, branchScale * 0.3f, branchScale * 0.1f);
	}

	private void GrowFruit(float fruitScale)
	{
		fruit.SetAge(fruitScale);
	}

	private Fruit CreateFruit()
	{
		//create offspring fruit with randomized genome
		GameObject fruitObj = Instantiate(fruitPrefab, fruitPos.position, Quaternion.identity);
		Fruit fruit = fruitObj.GetComponent<Fruit>();

		fruit.SetMutatedGenome(genome);

		return fruit;
	}

}
