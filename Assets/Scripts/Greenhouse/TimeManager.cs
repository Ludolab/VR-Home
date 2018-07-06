using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

	public static TimeManager instance;

	public GameObject neighborsObj;

	private int day = 0;

	private Neighbor[] neighbors;
	private Outbox[] outboxes;
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
		outboxes = FindObjectsOfType<Outbox>();
		ProcessDay();
	}

	private void ProcessDay()
	{
		foreach (Outbox outbox in outboxes)
		{
			Fruit[] contents = outbox.ClearFruit();
			if (contents.Length > 0)
			{
				string name = outbox.GetLabel();
				if (name != null)
				{
					Neighbor receiver = GetNeighborByName(name);
					receiver.GiveGift(contents);
				}
			}

			if (day == 1)
			{
				outbox.gameObject.SetActive(true);
			}
		}

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

	public void AddOutboxLabel(NeighborInfo info)
	{
		foreach (Outbox outbox in outboxes)
		{
			outbox.AddLabel(info);
		}
	}

	private Neighbor GetNeighborByName(string name)
	{
		return neighbors.First(n => n.info.name == name);
	}
}
