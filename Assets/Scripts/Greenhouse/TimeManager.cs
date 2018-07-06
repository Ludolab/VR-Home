using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

	public static TimeManager instance;

	public GameObject neighborsObj;

	private int day = 0;

	private Neighbor[] neighbors;
	private List<Plant> plants = new List<Plant>();

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("Should not have more than one TimeManager in a scene!");
		}
		instance = this;
	}

	private void Start()
	{
		neighbors = neighborsObj.GetComponentsInChildren<Neighbor>();
		ProcessDay();
	}

	private void ProcessDay()
	{
		//TODO: clear outbox
		//TODO: tell neighbors about outbox contents
		foreach (Neighbor neighbor in neighbors)
		{
			neighbor.StartDay(day);
		}

		foreach (Plant plant in plants)
		{
			plant.StartDay(day);
		}
	}

	public int GetDay()
	{
		return day;
	}

	[ContextMenu("Next Day")]
	public void NextDay()
	{
		day++;

		ProcessDay();
	}

	public void AddPlant(Plant plant)
	{
		plants.Add(plant);
	}
}
