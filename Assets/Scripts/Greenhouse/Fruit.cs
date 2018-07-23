using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Genome
{
	public string name; //name of plant

	public Color color1; //top color

	public Color color2; //bottom color

	public float scale; //overall size

	public float stretchScale; //y dimension size

	public float lifetime; //time (seconds) to reach maximum age

	//TODO: more fruit/plant properties...
}

public class Fruit : MonoBehaviour
{
	public Color unripeColor;
	public GameObject particlePrefab;
	public bool squishy; //whether to make a squishing sound when grabbing
	public AudioClip[] grabSounds;

	private bool picked = false;
	private ConfigurableJoint joint;
	private InteractionBehaviour ib;
	private Material mat;
	private AudioSource audioSrc;

	private Genome genome;

    private Plot plotIn;
	
	private void Awake()
	{
		audioSrc = GetComponent<AudioSource>();
		joint = GetComponent<ConfigurableJoint>();
		ib = GetComponent<InteractionBehaviour>();
		//mat = GetComponent<Renderer>().materials[1];
	}

	public void SetGenome(Genome g)
	{
		genome = g;

		//apply genome properties to gameobject
		//ApplyProperties(genome.color1, genome.color2, genome.scale, genome.stretchScale);
	}

	/*public Genome GetGenome()
	{
		return genome;
	}*/

	[ContextMenu("Grab")]
	public void Grab()
	{
		if (squishy)
		{
			audioSrc.PlayOneShot(grabSounds[Random.Range(0, grabSounds.Length)]);
		}
	}

	[ContextMenu("Pick")]
	public void Pick()
	{
		if (!picked)
		{
			SpawnParticles();
			Destroy(joint);
			if (plotIn != null) plotIn.RemoveFromFruits(gameObject);
			StartCoroutine(RefreshLocked());
			TimeManager.instance.AddGarbage(gameObject);
		}
	}

	private void SpawnParticles()
	{
		Instantiate(particlePrefab, transform.position, Quaternion.identity); //TODO: at anchor position?
	}

	private IEnumerator RefreshLocked()
	{
		yield return null;
		ib.RefreshPositionLockedState();
	}

	/* private void OnTriggerEnter(Collider other)
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
	}*/

	public void SetAge(float age)
	{
		//age is from 0 to 1

		//make less green and bigger
		Color c1 = Color.Lerp(unripeColor, genome.color1, age);
		Color c2 = Color.Lerp(unripeColor, genome.color2, age);
		float s = Mathf.Lerp(0, genome.scale, age);
		float ss = Mathf.Lerp(0, genome.stretchScale, age);
		//ApplyProperties(c1, c2, s, ss);
	}

	/*private void ApplyProperties(Color color1, Color color2, float scale, float stretchScale)
	{
		mat.SetColor("_Color1", color1);
		mat.SetColor("_Color2", color2);
		transform.localScale = new Vector3(scale, stretchScale, scale);

		//force anchor recalculation
		Vector3 anchor = joint.anchor;
		anchor.y = 0.5f;
		joint.anchor = anchor;
	}*/

	public void SetMutatedGenome(Genome g)
	{
		Genome mutated = MutateGenome(g);
		SetGenome(mutated);
	}

	private static Genome MutateGenome(Genome g)
	{
		//TODO: maybe randomize some properties a little bit
		/*if (Random.value < mutateChance)
		{
			//randomize name
			//with chance mutate each property
		}*/

		return g;
	}

	/*public string GetName()
	{
		return genome.name;
	}*/

    public void setPlot(Plot plot) {
        plotIn = plot;
    }
}
