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

	private ConfigurableJoint joint;
	private Rigidbody rb;
	private InteractionBehaviour ib;
	private Material mat;

	private Genome genome;
	private float anchorY;

	private void Awake()
	{
		joint = GetComponent<ConfigurableJoint>();
		rb = GetComponent<Rigidbody>();
		ib = GetComponent<InteractionBehaviour>();
		mat = GetComponent<Renderer>().material;
	}

	private void Start()
	{
		anchorY = transform.position.y;
	}

	public void SetGenome(Genome g)
	{
		genome = g;

		//apply genome properties to gameobject
		ApplyProperties(genome.color1, genome.color2, genome.scale, genome.stretchScale);
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

	public void SetAge(float age)
	{
		//age is from 0 to 1

		//make less green and bigger
		Color c1 = Color.Lerp(unripeColor, genome.color1, age);
		Color c2 = Color.Lerp(unripeColor, genome.color2, age);
		float s = Mathf.Lerp(0, genome.scale, age);
		float ss = Mathf.Lerp(0, genome.stretchScale, age);
		ApplyProperties(c1, c2, s, ss);

		/*rb.isKinematic = true;
		Vector3 pos = rb.position;
		pos.y = Mathf.Lerp(anchorY, anchorY - genome.stretchScale / 2, age);
		rb.position = pos;
		rb.isKinematic = false;*/
		Vector3 anchor = joint.anchor;
		anchor.y = ss / 2;
		joint.anchor = anchor;
		//TODO: fix fruit pos- keep anchor the same but move fruit relative to it
	}

	private void ApplyProperties(Color color1, Color color2, float scale, float stretchScale)
	{
		mat.SetColor("_Color1", color1);
		mat.SetColor("_Color2", color2);
		transform.localScale = new Vector3(scale, stretchScale, scale);
	}

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
}
