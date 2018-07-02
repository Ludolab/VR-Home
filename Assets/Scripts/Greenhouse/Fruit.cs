using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Genome
{
	public string name;

	public Color color1;

	public Color color2;

	public float scale;

	public float stretchScale;

	//TODO: more fruit/plant properties...
}

public class Fruit : MonoBehaviour
{

	private ConfigurableJoint joint;
	private InteractionBehaviour ib;
	private Material mat;

	private Genome genome;

	private void Awake()
	{
		joint = GetComponent<ConfigurableJoint>();
		ib = GetComponent<InteractionBehaviour>();
		mat = GetComponent<Renderer>().material;
	}

	public void SetGenome(Genome g)
	{
		genome = g;

		//apply genome properties to gameobject
		mat.SetColor("_Color1", genome.color1);
		mat.SetColor("_Color2", genome.color2);
		transform.localScale = new Vector3(genome.scale, genome.stretchScale, genome.scale);
	}

	public Genome GetGenome()
	{
		return genome;
	}

	public void Pick()
	{
		//TODO: sound, particles
		Destroy(joint);
		StartCoroutine(RefreshLocked());
	}

	private IEnumerator RefreshLocked()
	{
		yield return null;
		ib.RefreshPositionLockedState();
	}

	private void OnTriggerEnter(Collider other)
	{
		Pot pot = other.GetComponent<Pot>();
		if (pot != null)
		{
			PlantInPot(pot);
		}
	}

	private void PlantInPot(Pot pot)
	{
		pot.PlantFruit(this);
		Destroy(gameObject);
	}
}
